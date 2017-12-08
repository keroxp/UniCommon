#import <Foundation/Foundation.h>

extern "C" {
    static inline char* Hexat_Make_String_Copy (const char* string)
    {
        if (string == NULL) return NULL;
        char* res = (char*)malloc(strlen(string) + 1);
        strcpy(res, string);
        return res;
    }
    static const char* Hexat_Secret = "password__";
    const char* Hexat_Secret_iOS() {
        return Hexat_Make_String_Copy(Hexat_Secret);
    }
}
