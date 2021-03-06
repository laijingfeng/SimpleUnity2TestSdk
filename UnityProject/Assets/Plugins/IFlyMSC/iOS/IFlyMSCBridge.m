﻿#import "IFlyMSCBridge.h"

@implementation IFlyMSCBridge

static IFlyMSCBridge* gameMgr = nil; 

+ (IFlyMSCBridge*) instance  
{
	if (gameMgr == nil)  
	{
		gameMgr = [[IFlyMSCBridge alloc]init];  
	}
	return gameMgr;  
}

-(void)initRecognizer  
{
    NSLog(@"laijingfeng %s",__func__);
	//recognition singleton without view
	if (_iFlySpeechRecognizer == nil) {
		_iFlySpeechRecognizer = [IFlySpeechRecognizer sharedInstance];
	}
	[_iFlySpeechRecognizer setParameter:@"" forKey:[IFlySpeechConstant PARAMS]];
    //set recognition domain
	[_iFlySpeechRecognizer setParameter:@"iat" forKey:[IFlySpeechConstant IFLY_DOMAIN]];
	_iFlySpeechRecognizer.delegate = self;
	if (_iFlySpeechRecognizer != nil) {
		//set timeout of recording
		[_iFlySpeechRecognizer setParameter:@"30000" forKey:[IFlySpeechConstant SPEECH_TIMEOUT]];
		//set VAD timeout of end of speech(EOS)
		[_iFlySpeechRecognizer setParameter:@"3000" forKey:[IFlySpeechConstant VAD_EOS]];
		//set VAD timeout of beginning of speech(BOS)
		[_iFlySpeechRecognizer setParameter:@"3000" forKey:[IFlySpeechConstant VAD_BOS]];
		//set network timeout
		[_iFlySpeechRecognizer setParameter:@"20000" forKey:[IFlySpeechConstant NET_TIMEOUT]];
		//set sample rate, 16K as a recommended option
		[_iFlySpeechRecognizer setParameter:@"16000" forKey:[IFlySpeechConstant SAMPLE_RATE]];
		//set language
		[_iFlySpeechRecognizer setParameter:@"zh_cn" forKey:[IFlySpeechConstant LANGUAGE]];
		//set accent
		[_iFlySpeechRecognizer setParameter:@"mandarin" forKey:[IFlySpeechConstant ACCENT]];
		//set whether or not to show punctuation in recognition results
		[_iFlySpeechRecognizer setParameter:@"1" forKey:[IFlySpeechConstant ASR_PTT]];
	}
}

/**
 parse JSON data
 params,for example：
 {"sn":1,"ls":true,"bg":0,"ed":0,"ws":[{"bg":0,"cw":[{"w":"白日","sc":0}]},{"bg":0,"cw":[{"w":"依山","sc":0}]},{"bg":0,"cw":[{"w":"尽","sc":0}]},{"bg":0,"cw":[{"w":"黄河入海流","sc":0}]},{"bg":0,"cw":[{"w":"。","sc":0}]}]}
 **/
+ (NSString *)stringFromJson:(NSString*)params
{
    if (params == NULL) {
        return nil;
    }
    
    NSMutableString *tempStr = [[NSMutableString alloc] init];
    NSDictionary *resultDic  = [NSJSONSerialization JSONObjectWithData:
                                [params dataUsingEncoding:NSUTF8StringEncoding] options:kNilOptions error:nil];

    if (resultDic!= nil) {
        NSArray *wordArray = [resultDic objectForKey:@"ws"];
        
        for (int i = 0; i < [wordArray count]; i++) {
            NSDictionary *wsDic = [wordArray objectAtIndex: i];
            NSArray *cwArray = [wsDic objectForKey:@"cw"];
            
            for (int j = 0; j < [cwArray count]; j++) {
                NSDictionary *wDic = [cwArray objectAtIndex:j];
                NSString *str = [wDic objectForKey:@"w"];
                [tempStr appendString: str];
            }
        }
    }
    return tempStr;
}

-(void) startVoice
{
	NSLog(@"laijingfeng StartVoice");
	
	if(_iFlySpeechRecognizer == nil)
    {
        [self initRecognizer];
    }
    
	self.isCanceled = NO;
	
	[_iFlySpeechRecognizer cancel];
	//Set microphone as audio source
    [_iFlySpeechRecognizer setParameter:IFLY_AUDIO_SOURCE_MIC forKey:@"audio_source"];
	//Set result type
    [_iFlySpeechRecognizer setParameter:@"json" forKey:[IFlySpeechConstant RESULT_TYPE]];
    
	//Set the audio name of saved recording file while is generated in the local storage path of SDK,by default in library/cache.
	//[_iFlySpeechRecognizer setParameter:@nil forKey:[IFlySpeechConstant ASR_AUDIO_PATH]];//无需保存
	[_iFlySpeechRecognizer setParameter:@"asr.pcm" forKey:[IFlySpeechConstant ASR_AUDIO_PATH]];
	
    [_iFlySpeechRecognizer setDelegate:self];
    BOOL ret = [_iFlySpeechRecognizer startListening];
	if(ret){
		NSLog(@"laijingfeng StartVoice yes");
	}else{
		NSLog(@"laijingfeng StartVoice no");
	}
}

//开始录音回调
- (void) onBeginOfSpeech
{
	NSLog(@"laijingfeng %s", __func__);
}

//音量回调函数
- (void) onVolumeChanged:(int)volume
{
	NSLog(@"laijingfeng %s %d", __func__, volume);
}

/**
 result callback of recognition without view
 results：recognition results
 isLast：whether or not this is the last result
 **/
- (void) onResults:(NSArray *) results isLast:(BOOL)isLast  
{
	NSLog(@"laijingfeng %s", __func__);
    NSMutableString *resultString = [[NSMutableString alloc] init];  
    NSDictionary *dic = results[0];
    for (NSString *key in dic) {
        [resultString appendFormat:@"%@",key];
    }
	
    NSString *temp = [[NSString alloc] init];//textView
    _result = [NSString stringWithFormat:@"%@%@", temp, resultString];
	
    NSString * resultFromJson =  [IFlyMSCBridge stringFromJson:resultString];  
    temp = [NSString stringWithFormat:@"%@%@", temp, resultFromJson];  
    
	if (isLast){  
        NSLog(@"听写结果(json)：%@测试",  self.result);  
    }  
    UnitySendMessage("SDKMgr", "SDK2Unity_IFlyMSCCallback", [temp UTF8String]);  
}

//识别会话结束返回代理
- (void) onError:(IFlySpeechError *) error  
{
    NSLog(@"laijingfeng %s", __func__);
    NSString *text;
    if (self.isCanceled){
        text = @"识别取消";
    } else if (error.errorCode == 0 ){
        if (_result.length == 0){
            text = @"无识别结果";
        }else {
            text = @"识别成功";
        }
    }else {
        text = [NSString stringWithFormat:@"发生错误：%d %@", error.errorCode,error.errorDesc];
    }
	NSLog(@"%@",text);  
}

//会话取消回调
- (void) onCancel
{
	NSLog(@"laijingfeng %s", __func__);
}

//停止录音回调
- (void) onEndOfSpeech
{
	NSLog(@"laijingfeng %s", __func__);
}

/**
 stop recording
 **/
-(void) stopVoice  
{  
    NSLog(@"laijingfeng %s",__func__);
    
    [_iFlySpeechRecognizer stopListening];
}

/**
 cancel speech recognition
 **/
-(void) cancelVoice  
{  
    NSLog(@"%s",__func__);
    
    self.isCanceled = YES;

    [_iFlySpeechRecognizer cancel];
}

#if defined (__cplusplus)
extern "C"
{
#endif

	void __registerVoice()
	{
		[IFlyMSCBridge instance];
	}
	
	void __startVoice()
	{
		[gameMgr startVoice];
	}
	
	void __stopVoice()
	{  
		[gameMgr stopVoice];  
	}  
	
	void __cancelVoice()
	{  
		[gameMgr cancelVoice];  
	}

#if defined (__cplusplus)
}
#endif

@end