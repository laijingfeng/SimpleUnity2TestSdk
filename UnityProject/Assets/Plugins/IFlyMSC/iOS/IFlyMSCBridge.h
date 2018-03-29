#import <Foundation/Foundation.h>
#import "IFlyMSC/IFlyMSC.h"

@class IFlySpeechRecognizer;
//需要实现IFlyRecognizerViewDelegate识别协议
@interface IFlyMSCBridge : NSObject<IFlySpeechRecognizerDelegate>
//不带界面的识别对象
@property (nonatomic, strong) IFlySpeechRecognizer *iFlySpeechRecognizer;
@property (nonatomic, strong) NSString * result;
@property (nonatomic, assign) BOOL isCanceled;
+(IFlyMSCBridge*) instance;

#if defined (__cplusplus)
extern "C"
{
#endif

    void __registerVoice(void);
	void __startVoice(void);
	void __stopVoice(void);
	void __cancelVoice(void);

#if defined (__cplusplus)
}
#endif

@end