
    BEGIN TRAN T1

    INSERT INTO [kenuser].[IdentityProof]
           ([PassportNumber]
           ,[DateOfIssue]
           ,[DateOfExpiry]
           ,[PlaceOfIssue]
           ,[DrivingLicenseNo]
           ,[DLDateOfIssue]
           ,[DLDateOfExpiry]
           ,[DLPlaceOfIssue]
           ,[TypeOfDocument]
           ,[OtherDocNumber]
           ,[OtherDocDateOfIssue]
           ,[OtherDocDateOfExpiry]
           ,[NationalIDNumber]
           ,[NationalIDTypeCode]
           ,[NatIDPlaceOfIssue]
           ,[NatIDDateOfIssue]
           ,[NatIDDateOfExpiry]
           ,[ContractNumber]
           ,[ContractPlaceOfIssue]
           ,[ContractDateOfIssue]
           ,[ContractDateOfExpiry]
           ,[VisaNumber]
           ,[VisaTypeCode]
           ,[VisaCountryCode]
           ,[Profession]
           ,[Sponsor]
           ,[VisaDateOfIssue]
           ,[VisaDateOfExpiry]
           ,[EmployeeNo]
           ,[TransactionNo])
    SELECT  'P1234587B' AS PassportNumber, 
            '01/01/2020' AS DateOfIssue, 
            '12/31/2030' AS DateOfExpiry, 
            'Manama Bahrain' AS PlaceOfIssue,
            '781202645' AS DrivingLicenseNo, 
            '08/14/2019' AS DLDateOfIssue, 
            '08/14/2025' AS DLDateOfExpiry, 
            'Manama Bahrain' AS DLPlaceOfIssue, 
            'Work Portfolio' AS TypeOfDocument,
            'ABC12345' AS OtherDocNumber, 
            '10/25/2023' AS OtherDocDateOfIssue, 
            '12/31/2026' AS OtherDocDateOfExpiry, 
            '781202647' AS NationalIDNumber, 
            'PH' AS NationalIDTypeCode, 
            'Manila City' AS NatIDPlaceOfIssue, 
            '02/14/2018' AS NatIDDateOfIssue, 
            '12/31/2028' AS NatIDDateOfExpiry, 
            'CN123456' AS ContractNumber, 
            'Manama' AS ContractPlaceOfIssue, 
            '10/17/2023' AS ContractDateOfIssue, 
            '10/16/2025' AS ContractDateOfExpiry, 
            'VT11224455' AS VisaNumber, 
            'VTWORK' AS VisaTypeCode, 
            'PH' AS VisaCountryCode, 
            'Software Engineer' AS Profession, 
            'GARMCO' AS Sponsor, 
            '10/17/2023' AS VisaDateOfIssue, 
            '10/16/2025' AS VisaDateOfExpiry, 
            10003632 AS EmployeeNo, 
            NULL AS TransactionNo

    --ROLLBACK TRAN T1
    COMMIT TRAN T1



