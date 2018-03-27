#import <UIKit/UIKit.h>  
#import "iflyMSC/IFlyMSC.h"

@class IFlySpeechRecognizer;
@class IFlyPcmRecorder;

//需要实现IFlyRecognizerViewDelegate识别协议
@interface IFlyMSCBridge : UIViewController<IFlySpeechRecognizerDelegate>
//不带界面的识别对象
@property (nonatomic, strong) IFlySpeechRecognizer *iFlySpeechRecognizer;
@property (nonatomic, strong) NSString * result;
@property (nonatomic, assign) BOOL isCanceled;
@property (nonatomic,strong) IFlyPcmRecorder *pcmRecorder;//PCM Recorder to be used to demonstrate Audio Stream Recognition.
@property (nonatomic,assign) BOOL isStreamRec;//Whether or not it is Audio Stream function
@property (nonatomic,assign) BOOL isBeginOfSpeech;//Whether or not SDK has invoke the delegate methods of beginOfSpeech.
+(IFlyMSCBridge*) instance;

#if defined (__cplusplus)
extern "C"
{
#endif

    void __startup(void);
	void __startVoice(void);
	void __closeVoice(void);

#if defined (__cplusplus)
}
#endif

@end