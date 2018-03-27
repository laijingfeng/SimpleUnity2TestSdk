#import <UIKit/UIKit.h>  
#import "iflyMSC/IFlyMSC.h"

@interface IFlyMSCBridge : UIViewController<IFlySpeechRecognizerDelegate,IFlyRecognizerViewDelegate,UIActionSheetDelegate>

@property (nonatomic, strong) IFlySpeechRecognizer *iFlySpeechRecognizer;
@property (nonatomic, strong) NSString * result;
@property (nonatomic, assign) BOOL isCanceled;

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