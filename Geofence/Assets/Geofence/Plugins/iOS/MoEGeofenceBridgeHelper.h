//
//  MoEGeofenceBridgeHelper.h
//  MoEngage
//

#import <Foundation/Foundation.h>

@interface MoEGeofenceBridgeHelper : NSObject

+ (void)startGeofenceMonitoring:(NSDictionary *)payload;
+ (void)stopGeofenceMonitoring:(NSDictionary *)payload;

@end
