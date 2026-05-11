//
//  MoEngageUnityUtils.h
//  Unity-iPhone
//
//  Created by Rakshitha on 28/06/23.
//
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface MoEngageUnityUtils : NSObject

+(BOOL)isUnityAppControllerSwizzlingEnabled;
+(NSString* _Nullable)dictToJson:(NSDictionary *_Nullable)dict;
@end
