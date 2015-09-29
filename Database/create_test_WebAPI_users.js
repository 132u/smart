var account = db.Auth.AccountUsers.findOne({"email":"teamcity@mailforspam.com", "surname":"Test"});

db.Integration.ClientAuth.save({ 
    "_id" : "testapi", 
    "type" : "commonIntegrationApi", 
    "secretHash" : "QEutfZ3NH5niTWKPRpIV6mycumk1DpPmZfpkQJitfpAUDITisDlf4qVHJolDWYlsNDSid8YsM8H0LF56AMMiWQ==", 
    "accountId" : account.accountId, 
    "defaultVendorPrices" : {
        "rates" : [
            {
                "sourceLanguage" : "en", 
                "targetLanguage" : "fr", 
                "rate" : 8.9
            }, 
            {
                "sourceLanguage" : "en", 
                "targetLanguage" : "de", 
                "rate" : 8.9
            }, 
            {
                "sourceLanguage" : "en", 
                "targetLanguage" : "it", 
                "rate" : 8.9
            }, 
            {
                "sourceLanguage" : "en", 
                "targetLanguage" : "es", 
                "rate" : 8.9
            }, 
            {
                "sourceLanguage" : "en", 
                "targetLanguage" : "zh-CN", 
                "rate" : 6.9
            }, 
            {
                "sourceLanguage" : "en", 
                "targetLanguage" : "ko", 
                "rate" : 8.9
            }, 
            {
                "sourceLanguage" : "en", 
                "targetLanguage" : "ja", 
                "rate" : 10.9
            }
        ], 
        "priceComponents" : [
            {
                "field" : "102", 
                "discountPercent" : NumberInt(70)
            }, 
            {
                "field" : "101", 
                "discountPercent" : NumberInt(70)
            }, 
            {
                "field" : "100", 
                "discountPercent" : NumberInt(70)
            }, 
            {
                "field" : "Repetitions", 
                "discountPercent" : NumberInt(70)
            }, 
            {
                "field" : "95-99", 
                "discountPercent" : NumberInt(30)
            }, 
            {
                "field" : "85-94", 
                "discountPercent" : NumberInt(30)
            }, 
            {
                "field" : "75-84", 
                "discountPercent" : NumberInt(30)
            }, 
            {
                "field" : "New", 
                "discountPercent" : NumberInt(0)
            }
        ]
    }
});

db.Integration.ClientAuth.save({ 
    "_id" : "EasyTranslate_api_test", 
    "type" : "EasyTranslate", 
    "secretHash" : "32FLF+qIkuv7pI1b/UDyTXNzoFxS75RnG4at6FNKoFof8fpjrhUh3KN3vqd8h258lOXpX0YrLJJhpN3KLS2vTg==", 
    "defaultVendorAccountId" : account.accountId
});

db.Integration.ClientAuth.save({ 
    "_id" : "HotFolder_api_test", 
    "type" : "hotfolder", 
    "secretHash" : "VvdtZ3gvtKHZ8qDOY9oYxqZS09TUr+tiNwBf8ZZnVj354QITnhHlx0x/NdPseDQxfGULzTPMnkDc7782wZ/HiA==", 
    "accountId" : account.accountId
});