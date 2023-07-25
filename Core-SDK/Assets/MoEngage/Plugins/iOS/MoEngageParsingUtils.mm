//
//  MoEngageParsingUtils.m
//  UnityFramework
//
//  Created by Rakshitha on 28/06/23.
//

#import <Foundation/Foundation.h>


NSString* getNSStringFromChar(const char* str) {
    return str != NULL ? [NSString stringWithUTF8String:str] : [NSString stringWithUTF8String:""];
}

NSMutableDictionary* getDictionaryFromJSON(const char* jsonString) {
    
    NSMutableDictionary *dict = [NSMutableDictionary dictionaryWithCapacity:1];
    
    if (jsonString != NULL && jsonString != nil) {
        NSError *jsonError;
        NSData *objectData = [getNSStringFromChar(jsonString) dataUsingEncoding:NSUTF8StringEncoding];
        dict = [NSJSONSerialization JSONObjectWithData:objectData
                                               options:NSJSONReadingMutableContainers
                                                 error:&jsonError];
    }
    
    return dict;
}

