#import "IFlyMSCBridge.h"

@implementation IFlyMSCBridge

static IFlyMSCBridge* gameMgr = nil; 

+ (IFlyMSCBridge*) instance  
{
	if (gameMgr==nil)  
	{
		gameMgr=[[IFlyMSCBridge alloc]init];  
	}
	return gameMgr;  
}

-(void)initRecognizer  
{  
    NSLog(@"%s",__func__);  
    if (_iFlySpeechRecognizer == nil) {  
        _iFlySpeechRecognizer = [IFlySpeechRecognizer sharedInstance];
        [_iFlySpeechRecognizer setParameter:@"" forKey:[IFlySpeechConstant PARAMS]];
        [_iFlySpeechRecognizer setParameter:@"iat" forKey:[IFlySpeechConstant IFLY_DOMAIN]];
    }
    _iFlySpeechRecognizer.delegate = self;
    if (_iFlySpeechRecognizer != nil){
        [_iFlySpeechRecognizer setParameter:@"30000" forKey:[IFlySpeechConstant SPEECH_TIMEOUT]];  
        [_iFlySpeechRecognizer setParameter:@"500" forKey:[IFlySpeechConstant VAD_EOS]];  
        [_iFlySpeechRecognizer setParameter:@"10000" forKey:[IFlySpeechConstant VAD_BOS]];  
        [_iFlySpeechRecognizer setParameter:@"5000" forKey:[IFlySpeechConstant NET_TIMEOUT]];  
        [_iFlySpeechRecognizer setParameter:@"16000" forKey:[IFlySpeechConstant SAMPLE_RATE]];  
        [_iFlySpeechRecognizer setParameter:@"zh_cn" forKey:[IFlySpeechConstant LANGUAGE]];  
        [_iFlySpeechRecognizer setParameter:@"0" forKey:[IFlySpeechConstant ASR_PTT]];  
    }     
}

- (NSString *)stringFromJson:(NSString*)params  
{
    if (params == NULL){
        return nil;
    }

    NSMutableString *tempStr = [[NSMutableString alloc] init];  
    NSDictionary *resultDic  = [NSJSONSerialization JSONObjectWithData: [params dataUsingEncoding:NSUTF8StringEncoding] options:kNilOptions error:nil];  

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

- (void) onError:(IFlySpeechError *) error  
{  
    NSLog(@"%s",__func__);
    NSString *text;
    if (self.isCanceled) {  
        text = @"识别取消";
    } else if (error.errorCode == 0 ) {  
        if (_result.length == 0) {  
            text = @"无识别结果";  
        }else {  
            text = @"识别成功";  
        }  
    }else {  
        text = [NSString stringWithFormat:@"发生错误：%d %@", error.errorCode,error.errorDesc];  
        NSLog(@"%@",text);  
    }  
}

- (void) onResults:(NSArray *) results isLast:(BOOL)isLast  
{  
    NSMutableString *resultString = [[NSMutableString alloc] init];  
    NSDictionary *dic = results[0];
    for (NSString *key in dic) {
        [resultString appendFormat:@"%@",key];
    }

    NSString *temp = [[NSString alloc] init];  
      
    _result =[NSString stringWithFormat:@"%@%@", temp,resultString];  
    NSString * resultFromJson =  [gameMgr stringFromJson:resultString];  
    temp = [NSString stringWithFormat:@"%@%@", temp,resultFromJson];  
    
	if (isLast){  
        NSLog(@"听写结果(json)：%@测试",  self.result);  
    }  
    NSLog(@"_result=%@",_result);  
    NSLog(@"resultFromJson=%@",resultFromJson);  
    NSLog(@"isLast=%d,_textView.text=%@",isLast,temp);  
    char* str1 = [temp UTF8String];
    UnitySendMessage("SDKMgr", "SDK2Unity_IFlyMSCCallback", str1);  
}

-(void) StartVoice
{
    if(_iFlySpeechRecognizer == nil)
    {
        [self initRecognizer];
    }
    
    [_iFlySpeechRecognizer cancel];
    [_iFlySpeechRecognizer setParameter:IFLY_AUDIO_SOURCE_MIC forKey:@"audio_source"];
    [_iFlySpeechRecognizer setParameter:@"json" forKey:[IFlySpeechConstant RESULT_TYPE]];
    [_iFlySpeechRecognizer setParameter:@"asr.pcm" forKey:[IFlySpeechConstant ASR_AUDIO_PATH]];
    [_iFlySpeechRecognizer setDelegate:self];
    [_iFlySpeechRecognizer startListening];
}

-(void) closeVoice  
{  
    [_iFlySpeechRecognizer cancel];  
} 

#if defined (__cplusplus)
extern "C"
{
#endif

	void __startup()
	{
		[IFlyMSCBridge instance];
		NSString *initString = [[NSString alloc] initWithFormat:@"appid=%@",@"5ab91122"];
		[IFlySpeechUtility createUtility:initString];
	}
	
	void __startVoice()
	{
		[gameMgr StartVoice];
	}
	
	void __closeVoice()
	{  
		[gameMgr closeVoice];  
	}  

#if defined (__cplusplus)
}
#endif

@end