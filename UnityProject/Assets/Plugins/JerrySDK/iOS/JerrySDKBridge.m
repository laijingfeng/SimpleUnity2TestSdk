#import "JerrySDKBridge.h"

@implementation JerrySDKBridge

+ (int)Add:(int)a and:(int)b{
    return a + b;
}

@end

#if defined (__cplusplus)
extern "C"
{
#endif
    
    int AddC(int a, int b){
        return [JerrySDKBridge Add:a and:b];
    }
    
#if defined (__cplusplus)
}
#endif