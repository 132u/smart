USE [SmartCat_AutoTests]
SET QUOTED_IDENTIFIER ON
SET XACT_ABORT ON

INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('97CA8F26-431F-4CEE-BF9E-AB019C5E24C7', 'Bobby Test', 'bobby@mailforspam.com', 25, 1)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('9ACB3343-87B0-4468-94D4-640E437666DF', 'Ringo Star', 'ringo123@mailforspam.com', 25, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('31EACE84-8E0E-45CA-811E-AD60F6A73588', 'Иван Петров', 'testuserivanpetrov@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('C6BFCCAE-C724-4431-A335-52032057E8C4', 'Вася Сидоров', 'testuservasyasidorov@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('68A32D7C-BBF9-482E-B925-E15FCADB8195', 'Марина Олеговна', 'testusermarinaolegovna@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('1137150B-E381-4139-A6D2-5BF5E5361E99', 'Ольга Владимирова', 'testusermolgavladimirova@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('F2414E0E-EC5B-4F28-8FAE-403289518052', 'Павел Красов', 'testuserpavelkrasov@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('FA297884-0589-4165-8443-355A4D34993B', 'Николай Лебедев', 'testusernikolaylebedev@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('373D44A0-8979-4BBD-8A1D-5CC62B89D8B1', 'Эдгар Иванов', 'testuseredgarivanov@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('CF4F5679-4DC6-455F-BA15-5584D3D9D8B5', 'Федя Васильев', 'testuserfedyavasilyev@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('F6CA611F-5E67-4671-8328-04E6F8BA365A', 'Артур Соколов', 'testuserartursokolov@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('244ACCBF-8C11-4635-9E43-B1CD108B7FD3', 'Антон Борисов', 'testuserantonborisov@mailforspam.com.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('84D7D1A0-5343-41CF-8198-D9FC0845DFAB', 'Ритина Мамедова', 'testuserritinamamedova@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('22414F76-ED2C-457C-8E11-5B4B750A9BFB', 'Мария Титова', 'testusermariyatitova@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('8ABA9119-420F-476F-8FCA-5B822A17C122', 'Игорь Карпов', 'testuserigorkarpov@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('6AD4C0A3-B849-4B28-BEF1-564B22273A0A', 'Василиса Андреева', 'testuservasilisaandreeva@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('A30A9E04-6FFB-431E-B31D-ABDC076DD350', 'Наталья Мальцева', 'testusernatalyamalceva@mailforspam.com', 9, 0)
INSERT INTO Auth.Users (ID, NickName, EMail, PreferredLanguageID, IsAdmin)
VALUES ('2B346F27-6B8A-4692-BCFA-C0DC12F72F10', 'Олег Арцер', 'testuserolegarcer@mailforspam.com', 9, 0)


INSERT INTO Core.Accounts (ID, Name, SubDomain, IsActive, CreatedByUserID, DictionariesExpirationDate, FeatureType, CrowdUsersAreModerated, WorkflowEnabled, AccountType, VentureId)
VALUES ('0D6E5EFC-962A-488F-AB0D-FE4C8D1F28AF', 'TestAccount', 'test-acc', 1, '97CA8F26-431F-4CEE-BF9E-AB019C5E24C7', '2020-01-01', 65535, 0, 1, 3, 'SmartCAT')

INSERT INTO Core.Accounts (ID, Name, SubDomain, IsActive, CreatedByUserID, DictionariesExpirationDate, FeatureType, CrowdUsersAreModerated, WorkflowEnabled, AccountType, VentureId)
VALUES ('B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Coursera', 'coursera', 1, '97CA8F26-431F-4CEE-BF9E-AB019C5E24C7', '2020-01-01', 65535, 0, 1, 3, 'Coursera')

INSERT INTO Core.Accounts (ID, Name, SubDomain, IsActive, CreatedByUserID, DictionariesExpirationDate, FeatureType, CrowdUsersAreModerated, WorkflowEnabled, AccountType, VentureId)
VALUES ('1D53DFDF-7DF7-44A4-9396-6158DDD5D068', 'Perevedem', 'perevedem', 1, '97CA8F26-431F-4CEE-BF9E-AB019C5E24C7', '2020-01-01', 65535, 0, 1, 3, 'Perevedem.ru')


DECLARE @Ids TABLE
(
	AccountId uniqueidentifier NOT NULL,
	AccountUserId uniqueidentifier NOT NULL,
	GroupId uniqueidentifier NOT NULL
)

INSERT INTO @Ids (AccountId, AccountUserId, GroupId)
SELECT '0D6E5EFC-962A-488F-AB0D-FE4C8D1F28AF', '3A71D19D-4938-45F1-98A0-B3D01047A2BD', 'B148B86F-C9E5-4F48-971E-00C8AED85B25'
UNION ALL
SELECT '1D53DFDF-7DF7-44A4-9396-6158DDD5D068', '85A00720-6207-4A64-BC52-700E577AD1BF', '2F3F2C0F-9D30-468C-9585-8BE173ECF478'

INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
SELECT AccountUserId, '97CA8F26-431F-4CEE-BF9E-AB019C5E24C7', AccountId, 'Bobby', 'Test', 'bobby@mailforspam.com', 1, CAST(AccountUserId AS nvarchar(MAX)), '2020-01-01', GETDATE()
FROM @Ids

INSERT INTO Auth.AccountUsers_Groups (AccountUserId, GroupId)
SELECT AccountUserId, GroupId
FROM @Ids


DECLARE @Ids1 TABLE
(
	AccountId uniqueidentifier NOT NULL,
	AccountUserId uniqueidentifier NOT NULL,
	GroupId uniqueidentifier NOT NULL
)

INSERT INTO @Ids1 (AccountId, AccountUserId, GroupId)
SELECT '0D6E5EFC-962A-488F-AB0D-FE4C8D1F28AF', '0E2D9A40-33EB-4B49-8853-CD3398BD8A2B', 'A46AF1D7-4E34-437B-AABA-6EDC74E3E980'
UNION ALL
SELECT '1D53DFDF-7DF7-44A4-9396-6158DDD5D068', 'CD925B07-9989-4507-A1FF-63F4408D01B9', '08441933-24C7-4A0D-9CC8-B352B9663E3E'

INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
SELECT AccountUserId, '9ACB3343-87B0-4468-94D4-640E437666DF', AccountId, 'Ringo', 'Star', 'ringo123@mailforspam.com', 1, CAST(AccountUserId AS nvarchar(MAX)), '2020-01-01', GETDATE()
FROM @Ids1

INSERT INTO Auth.AccountUsers_Groups (AccountUserId, GroupId)
SELECT AccountUserId, GroupId
FROM @Ids1


INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('54E73D30-238A-4BD3-AFE5-094963FEA8BD', '31EACE84-8E0E-45CA-811E-AD60F6A73588', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Иван', 'Петров', 'testuserivanpetrov@mailforspam.com', 1, CAST('54E73D30-238A-4BD3-AFE5-094963FEA8BD' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('29F6423C-524E-4385-BE16-E0C22D714C3B', 'C6BFCCAE-C724-4431-A335-52032057E8C4', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Вася', 'Сидоров', 'testuservasyasidorov@mailforspam.com', 1, CAST('29F6423C-524E-4385-BE16-E0C22D714C3B' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('2C662D57-2DE0-467B-ABE4-E2ADC3854163', '68A32D7C-BBF9-482E-B925-E15FCADB8195', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Марина', 'Олеговна', 'testusermarinaolegovna@mailforspam.com', 1, CAST('2C662D57-2DE0-467B-ABE4-E2ADC3854163' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('F5175778-FB6A-4A9B-9872-0765E80B8B29', '1137150B-E381-4139-A6D2-5BF5E5361E99', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Ольга', 'Владимирова', 'testusermolgavladimirova@mailforspam.com', 1, CAST('F5175778-FB6A-4A9B-9872-0765E80B8B29' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('D4C2FB57-292E-4FCC-974C-914957822EAB', 'F2414E0E-EC5B-4F28-8FAE-403289518052', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Павел', 'Красов', 'testuserpavelkrasov@mailforspam.com', 1, CAST('D4C2FB57-292E-4FCC-974C-914957822EAB' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('883B2CC2-E430-48DB-9778-67CF6326EADE', 'FA297884-0589-4165-8443-355A4D34993B', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Николай', 'Лебедев', 'testusernikolaylebedev@mailforspam.com', 1, CAST('883B2CC2-E430-48DB-9778-67CF6326EADE' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('9EA17DC2-C6D8-4902-94E4-AC36406EECA3', '373D44A0-8979-4BBD-8A1D-5CC62B89D8B1', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Эдгар', 'Иванов', 'testuseredgarivanov@mailforspam.com', 1, CAST('9EA17DC2-C6D8-4902-94E4-AC36406EECA3' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('56917083-42B2-4A22-B836-768B7BF3F294', 'CF4F5679-4DC6-455F-BA15-5584D3D9D8B5', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Федя', 'Васильев', 'testuserfedyavasilyev@mailforspam.com', 1, CAST('56917083-42B2-4A22-B836-768B7BF3F294' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('5412D1D8-F1F0-49EC-B615-ED1538CC0ABA', 'F6CA611F-5E67-4671-8328-04E6F8BA365A', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Артур', 'Соколов', 'testuserartursokolov@mailforspam.com', 1, CAST('5412D1D8-F1F0-49EC-B615-ED1538CC0ABA' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('B01F559F-21A7-4CD7-85F5-BB7C2E4C66C0', '244ACCBF-8C11-4635-9E43-B1CD108B7FD3', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Антон', 'Борисов', 'testuserantonborisov@mailforspam.com.com', 1, CAST('B01F559F-21A7-4CD7-85F5-BB7C2E4C66C0' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('97A0A81D-15C5-45F5-9060-180B36ABBAC1', '84D7D1A0-5343-41CF-8198-D9FC0845DFAB', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Ритина', 'Мамедова', 'testuserritinamamedova@mailforspam.com', 1, CAST('97A0A81D-15C5-45F5-9060-180B36ABBAC1' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('720B1CF1-B986-4C43-8753-DE8C0D903352', '22414F76-ED2C-457C-8E11-5B4B750A9BFB', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Мария', 'Титова', 'testusermariyatitova@mailforspam.com', 1, CAST('720B1CF1-B986-4C43-8753-DE8C0D903352' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('C4F5B89F-9303-40EE-ADD0-A8339F87C209', '8ABA9119-420F-476F-8FCA-5B822A17C122', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Игорь', 'Карпов', 'testuserigorkarpov@mailforspam.com', 1, CAST('C4F5B89F-9303-40EE-ADD0-A8339F87C209' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('F8147B2A-E25E-42CF-81F0-0C9D5AA9C7E2', '6AD4C0A3-B849-4B28-BEF1-564B22273A0A', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Василиса', 'Андреева', 'testuservasilisaandreeva@mailforspam.com', 1, CAST('F8147B2A-E25E-42CF-81F0-0C9D5AA9C7E2' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('A37DEE1C-5F88-4820-83A7-C092E3C6FB22', 'A30A9E04-6FFB-431E-B31D-ABDC076DD350', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Наталья', 'Мальцева', 'testusernatalyamalceva@mailforspam.com', 1, CAST('A37DEE1C-5F88-4820-83A7-C092E3C6FB22' AS nvarchar(MAX)), '2020-01-01', GETDATE())
INSERT INTO Auth.AccountUsers (ID, UserID, AccountID, Name, Surname, EMail, [Status], InviteConfirmCode, InviteExpirationDate, ActivationDate)
VALUES ('9EA8EB81-BE3C-460F-93B1-A80FEC6835D0', '2B346F27-6B8A-4692-BCFA-C0DC12F72F10', 'B7776F45-05BE-4AFD-8EC5-706203BBB292', 'Олег', 'Арцер', 'testuserolegarcer@mailforspam.com', 1, CAST('9EA8EB81-BE3C-460F-93B1-A80FEC6835D0' AS nvarchar(MAX)), '2020-01-01', GETDATE())


INSERT INTO Cat.DisassembleDocumentMethods_Accounts (AccountId, MethodName)
SELECT AccountId, 'aspose'
FROM @Ids

INSERT INTO Cat.DisassembleDocumentMethods_Accounts (AccountId, MethodName)
SELECT AccountId, 'resx'
FROM @Ids

INSERT INTO Cat.DisassembleDocumentMethods_Accounts (AccountId, MethodName)
SELECT AccountId, 'csv iiko (.csv)'
FROM @Ids

INSERT INTO Cat.DisassembleDocumentMethods_Accounts (AccountId, MethodName)
SELECT AccountId, 'ttx'
FROM @Ids

INSERT INTO Cat.DisassembleDocumentMethods_Accounts (AccountId, MethodName)
SELECT AccountId, 'xliff'
FROM @Ids

INSERT INTO Cat.DisassembleDocumentMethods_Accounts (AccountId, MethodName)
SELECT AccountId, 'OpenXml'
FROM @Ids

INSERT INTO Cat.DisassembleDocumentMethods_Accounts (AccountId, MethodName)
SELECT AccountId, 'TractorOcr'
FROM @Ids
INSERT INTO Cat.DisassembleDocumentMethods_Accounts (AccountId, MethodName)
VALUES ('B7776F45-05BE-4AFD-8EC5-706203BBB292', 'aspose')

INSERT INTO Cat.DisassembleDocumentMethods_Accounts (AccountId, MethodName)
VALUES ('B7776F45-05BE-4AFD-8EC5-706203BBB292', 'resx')

INSERT INTO Cat.DisassembleDocumentMethods_Accounts (AccountId, MethodName)
VALUES ('B7776F45-05BE-4AFD-8EC5-706203BBB292', 'csv iiko (.csv)')

INSERT INTO Cat.DisassembleDocumentMethods_Accounts (AccountId, MethodName)
VALUES ('B7776F45-05BE-4AFD-8EC5-706203BBB292', 'ttx')

INSERT INTO Cat.DisassembleDocumentMethods_Accounts (AccountId, MethodName)
VALUES ('B7776F45-05BE-4AFD-8EC5-706203BBB292', 'xliff')
