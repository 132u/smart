db = connect("mongodb-autumn.als.local:27017/smartcat_autotests");
db.Auth.Groups.insert({
"_id" : CSUUID("B148B86F-C9E5-4F48-971E-00C8AED85B25"),
    "accountId" : CSUUID("0D6E5EFC-962A-488F-AB0D-FE4C8D1F28AF"),
    "name" : "Administrators",
    "accessRights" : [ 
        {
            "_t" : "PaidResourcesAccessRight"
        }, 
        {
            "_t" : "UsersManagementAccessRight"
        }, 
        {
            "_t" : "ClientsAndDomainsManagementAccessRight"
        }, 
        {
            "_t" : "AddSuggestsWithoutGlossaryAccessRight"
        }, 
        {
            "_t" : "CommentsModerationAccessRight"
        }, 
        {
            "_t" : "GlossaryManagementRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "GlossarySearchRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "GlossaryManagementRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "TMManagementRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "TMSearchRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "TMManagementRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "ProjectResourceManagementRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "ProjectCreationRight",
            "scopedByClientAndDomain" : true
        }
    ]
});

db.Auth.Groups.insert({
"_id" : CSUUID("2F3F2C0F-9D30-468C-9585-8BE173ECF478"),
    "accountId" : CSUUID("1D53DFDF-7DF7-44A4-9396-6158DDD5D068"),
    "name" : "Administrators",
    "accessRights" : [ 
        {
            "_t" : "PaidResourcesAccessRight"
        }, 
        {
            "_t" : "UsersManagementAccessRight"
        }, 
        {
            "_t" : "ClientsAndDomainsManagementAccessRight"
        }, 
        {
            "_t" : "AddSuggestsWithoutGlossaryAccessRight"
        }, 
        {
            "_t" : "CommentsModerationAccessRight"
        }, 
        {
            "_t" : "GlossaryManagementRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "GlossarySearchRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "GlossaryManagementRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "TMManagementRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "TMSearchRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "TMManagementRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "ProjectResourceManagementRight",
            "scopedByClientAndDomain" : true
        }, 
        {
            "_t" : "ProjectCreationRight",
            "scopedByClientAndDomain" : true
        }
    ]
});