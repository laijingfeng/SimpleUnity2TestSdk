#import <Foundation/Foundation.h>

@interface JerrySDKBridge : NSObject

@end

#if defined (__cplusplus)
extern "C"
{
#endif
    
    int AddC(int a, int b);
    
#if defined (__cplusplus)
}
#endif