
[Column(TypeName = "varchar(150)")]
public string? BankBranchName { get; set; } = null;

public byte PayGrade { get; set; } = 0;
public string PayGrade { get; set; } = null!;

[NotMapped]
public string PayGradeDesc { get; set; } = null!;

[Column(TypeName = "varchar(40)")]
public string JobTitle { get; set; } = null!;

[Column(TypeName = "varchar(20)")]
public string JobTitleCode { get; set; } = null!;

[NotMapped]
public string JobTitleDesc { get; set; } = null!;

public string? Company { get; set; } = null;

[Column(TypeName = "varchar(20)")]
public string? CompanyBranchCode { get; set; } = null;

[NotMapped]
public string? CompanyBranchDesc { get; set; } = null;

Before
[Column(TypeName = "varchar(20)")]
public string? PresentCityCode { get; set; } = null;

After
[Column(TypeName = "varchar(100)")]
public string? PresentCity { get; set; } = null;

Before
[Column(TypeName = "varchar(20)")]
public string? PermanentCityCode { get; set; } = null;

After
[Column(TypeName = "varchar(100)")]
public string? PermanentCity { get; set; } = null;

SearchVisaType