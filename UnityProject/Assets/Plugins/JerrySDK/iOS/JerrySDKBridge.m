#import "JerrySDKBridge.h"
#import <AdSupport/AdSupport.h>

@implementation JerrySDKBridge
@end

#if defined (__cplusplus)
extern "C"
{
#endif

	void __getIDFA(){
		const char* gameObject = "SDKMgr";
		const char* functionname = "SDK2Unity_GetIDFACallback";
		NSString *str = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
		UnitySendMessage(gameObject, functionname, [str UTF8String]);
	}

#if defined (__cplusplus)
}
#endif