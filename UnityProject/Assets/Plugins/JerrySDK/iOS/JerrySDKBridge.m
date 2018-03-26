#import "JerrySDKBridge.h"
#import <AdSupport/AdSupport.h>

@implementation JerrySDKBridge
void __getIDFA (){
    const char* gameObject = "SDKMgr";
    const char* functionname = "SDK2Unity_GetIDFACallback";
    NSString *str = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
    UnitySendMessage(gameObject, functionname, [str UTF8String]);
};
@end
