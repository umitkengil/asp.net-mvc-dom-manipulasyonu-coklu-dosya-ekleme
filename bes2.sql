USE [BES2]
GO
/****** Object:  User [IIS APPPOOL\panelbes]    Script Date: 27.09.2022 12:44:40 ******/
CREATE USER [IIS APPPOOL\panelbes] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [IIS APPPOOL\panelbes]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [IIS APPPOOL\panelbes]
GO
/****** Object:  Table [dbo].[bankTable]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bankTable](
	[bankID] [int] IDENTITY(1,1) NOT NULL,
	[bankCode] [nvarchar](max) NULL,
	[bankDescription] [nvarchar](max) NULL,
	[bankName] [nvarchar](max) NULL,
 CONSTRAINT [PK_bankTable] PRIMARY KEY CLUSTERED 
(
	[bankID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[storeTable]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[storeTable](
	[storeID] [int] IDENTITY(1,1) NOT NULL,
	[storeCode] [nvarchar](max) NULL,
	[storeName] [nvarchar](max) NULL,
	[storeDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_storeTable] PRIMARY KEY CLUSTERED 
(
	[storeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[expenceTable]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[expenceTable](
	[expenceID] [int] IDENTITY(1,1) NOT NULL,
	[expenceCode] [nvarchar](max) NULL,
	[expenceDescription] [nvarchar](max) NULL,
	[expenceBool] [bit] NULL,
 CONSTRAINT [PK_expenceTable] PRIMARY KEY CLUSTERED 
(
	[expenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[invoiceLogTable]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[invoiceLogTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Admin] [bit] NULL,
	[UserName] [nvarchar](250) NULL,
	[UpdateTime] [datetime] NULL,
	[invoiceId] [int] NULL,
	[store_id] [int] NULL,
	[newstore_id] [int] NULL,
	[expence_id] [int] NULL,
	[newexpence_id] [int] NULL,
	[bank_id] [int] NULL,
	[newbank_id] [int] NULL,
	[cash] [decimal](18, 2) NULL,
	[newcash] [decimal](18, 2) NULL,
	[credit] [decimal](18, 2) NULL,
	[newcredit] [decimal](18, 2) NULL,
	[amount] [decimal](18, 2) NOT NULL,
	[newamount] [decimal](18, 2) NOT NULL,
	[invoiceDate] [date] NOT NULL,
	[newinvoiceDate] [date] NOT NULL,
 CONSTRAINT [PK_invoiceLogTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vwInvoiceLog]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwInvoiceLog]
AS
SELECT dbo.invoiceLogTable.Id, dbo.invoiceLogTable.UserId, dbo.invoiceLogTable.Admin, dbo.invoiceLogTable.UserName, dbo.invoiceLogTable.UpdateTime, dbo.invoiceLogTable.invoiceId, dbo.invoiceLogTable.store_id, 
                  dbo.invoiceLogTable.newstore_id, dbo.invoiceLogTable.expence_id, dbo.invoiceLogTable.newexpence_id, dbo.invoiceLogTable.bank_id, dbo.invoiceLogTable.newbank_id, dbo.invoiceLogTable.cash, dbo.invoiceLogTable.newcash, 
                  dbo.invoiceLogTable.credit, dbo.invoiceLogTable.newcredit, dbo.invoiceLogTable.amount, dbo.invoiceLogTable.newamount, dbo.invoiceLogTable.invoiceDate, dbo.invoiceLogTable.newinvoiceDate, 
                  dbo.expenceTable.expenceCode AS newexpenceCode, expenceTable_1.expenceCode, storeTable_1.storeCode, dbo.storeTable.storeCode AS newstoreCode, dbo.bankTable.bankCode AS newbankCode, bankTable_1.bankCode
FROM     dbo.invoiceLogTable LEFT OUTER JOIN
                  dbo.bankTable ON dbo.invoiceLogTable.newbank_id = dbo.bankTable.bankID LEFT OUTER JOIN
                  dbo.bankTable AS bankTable_1 ON dbo.invoiceLogTable.bank_id = bankTable_1.bankID LEFT OUTER JOIN
                  dbo.expenceTable ON dbo.invoiceLogTable.newexpence_id = dbo.expenceTable.expenceID LEFT OUTER JOIN
                  dbo.expenceTable AS expenceTable_1 ON dbo.invoiceLogTable.expence_id = expenceTable_1.expenceID LEFT OUTER JOIN
                  dbo.storeTable ON dbo.invoiceLogTable.newstore_id = dbo.storeTable.storeID LEFT OUTER JOIN
                  dbo.storeTable AS storeTable_1 ON dbo.invoiceLogTable.store_id = storeTable_1.storeID
GO
/****** Object:  Table [dbo].[FileTables]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileTables](
	[fileID] [int] IDENTITY(1,1) NOT NULL,
	[store_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[fileURL] [nvarchar](max) NOT NULL,
	[fileName] [nvarchar](max) NOT NULL,
	[AddedDate] [datetime] NULL,
	[Status] [bit] NULL,
	[DeletedDate] [datetime] NULL,
 CONSTRAINT [PK_FileTables] PRIMARY KEY CLUSTERED 
(
	[fileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[invoiceTable]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[invoiceTable](
	[invoiceID] [int] IDENTITY(1,1) NOT NULL,
	[store_id] [int] NOT NULL,
	[bank_id] [int] NOT NULL,
	[expence_id] [int] NOT NULL,
	[cash] [decimal](18, 2) NULL,
	[credit] [decimal](18, 2) NULL,
	[amount] [decimal](18, 2) NULL,
	[expenceDescription] [nvarchar](max) NULL,
	[invoiceDate] [datetime] NOT NULL,
	[user_id] [int] NULL,
	[odemeTuruTablosu_id] [int] NOT NULL,
	[taksitSayisi] [tinyint] NULL,
 CONSTRAINT [PK_invoiceTable] PRIMARY KEY CLUSTERED 
(
	[invoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[odemeTuruTablosu]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[odemeTuruTablosu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[odemeTur] [nvarchar](10) NULL,
 CONSTRAINT [PK_paymentTypeTable] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[resfundTable]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[resfundTable](
	[refundID] [int] IDENTITY(1,1) NOT NULL,
	[store_id] [int] NULL,
	[user_id] [int] NULL,
	[bank_id] [int] NULL,
	[resfundType] [int] NULL,
	[resfundDescription] [nvarchar](max) NULL,
	[resfundFiles] [nvarchar](max) NULL,
	[resfundCash] [decimal](18, 3) NULL,
	[resfundDate] [datetime] NULL,
	[resfundCredit] [decimal](18, 3) NULL,
 CONSTRAINT [PK_refundTable] PRIMARY KEY CLUSTERED 
(
	[refundID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[resfundTypeTable]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[resfundTypeTable](
	[resfundTypeID] [int] IDENTITY(1,1) NOT NULL,
	[resfundTypeName] [nvarchar](50) NULL,
 CONSTRAINT [PK_refundTypeTable] PRIMARY KEY CLUSTERED 
(
	[resfundTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[roleTable]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[roleTable](
	[roleID] [int] IDENTITY(1,1) NOT NULL,
	[roleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_roleTable] PRIMARY KEY CLUSTERED 
(
	[roleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[userTable]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userTable](
	[userID] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](max) NOT NULL,
	[password] [nvarchar](max) NOT NULL,
	[role_id] [int] NOT NULL,
	[store_id] [int] NULL,
	[isOnline] [bit] NULL,
	[loginDate] [datetime] NULL,
	[logoutDate] [datetime] NULL,
	[user_position] [nvarchar](50) NULL,
	[cashier_code] [nvarchar](50) NULL,
 CONSTRAINT [PK_userTable] PRIMARY KEY CLUSTERED 
(
	[userID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[invoiceTable] ADD  CONSTRAINT [DF_invoiceTable_cash]  DEFAULT ((0)) FOR [cash]
GO
ALTER TABLE [dbo].[invoiceTable] ADD  CONSTRAINT [DF_invoiceTable_credit]  DEFAULT ((0)) FOR [credit]
GO
ALTER TABLE [dbo].[invoiceTable] ADD  CONSTRAINT [DF_invoiceTable_amount]  DEFAULT ((0)) FOR [amount]
GO
ALTER TABLE [dbo].[FileTables]  WITH CHECK ADD  CONSTRAINT [FK_FileTables_storeTable] FOREIGN KEY([store_id])
REFERENCES [dbo].[storeTable] ([storeID])
GO
ALTER TABLE [dbo].[FileTables] CHECK CONSTRAINT [FK_FileTables_storeTable]
GO
ALTER TABLE [dbo].[FileTables]  WITH CHECK ADD  CONSTRAINT [FK_FileTables_userTable] FOREIGN KEY([user_id])
REFERENCES [dbo].[userTable] ([userID])
GO
ALTER TABLE [dbo].[FileTables] CHECK CONSTRAINT [FK_FileTables_userTable]
GO
ALTER TABLE [dbo].[invoiceTable]  WITH CHECK ADD  CONSTRAINT [FK_invoiceTable_bankTable] FOREIGN KEY([bank_id])
REFERENCES [dbo].[bankTable] ([bankID])
GO
ALTER TABLE [dbo].[invoiceTable] CHECK CONSTRAINT [FK_invoiceTable_bankTable]
GO
ALTER TABLE [dbo].[invoiceTable]  WITH CHECK ADD  CONSTRAINT [FK_invoiceTable_expenceTable] FOREIGN KEY([expence_id])
REFERENCES [dbo].[expenceTable] ([expenceID])
GO
ALTER TABLE [dbo].[invoiceTable] CHECK CONSTRAINT [FK_invoiceTable_expenceTable]
GO
ALTER TABLE [dbo].[invoiceTable]  WITH CHECK ADD  CONSTRAINT [FK_invoiceTable_odemeTuruTablosu] FOREIGN KEY([odemeTuruTablosu_id])
REFERENCES [dbo].[odemeTuruTablosu] ([ID])
GO
ALTER TABLE [dbo].[invoiceTable] CHECK CONSTRAINT [FK_invoiceTable_odemeTuruTablosu]
GO
ALTER TABLE [dbo].[invoiceTable]  WITH CHECK ADD  CONSTRAINT [FK_invoiceTable_storeTable] FOREIGN KEY([store_id])
REFERENCES [dbo].[storeTable] ([storeID])
GO
ALTER TABLE [dbo].[invoiceTable] CHECK CONSTRAINT [FK_invoiceTable_storeTable]
GO
ALTER TABLE [dbo].[invoiceTable]  WITH CHECK ADD  CONSTRAINT [FK_invoiceTable_userTable] FOREIGN KEY([user_id])
REFERENCES [dbo].[userTable] ([userID])
GO
ALTER TABLE [dbo].[invoiceTable] CHECK CONSTRAINT [FK_invoiceTable_userTable]
GO
ALTER TABLE [dbo].[resfundTable]  WITH CHECK ADD  CONSTRAINT [FK_resfundTable_bankTable] FOREIGN KEY([bank_id])
REFERENCES [dbo].[bankTable] ([bankID])
GO
ALTER TABLE [dbo].[resfundTable] CHECK CONSTRAINT [FK_resfundTable_bankTable]
GO
ALTER TABLE [dbo].[resfundTable]  WITH CHECK ADD  CONSTRAINT [FK_resfundTable_resfundTypeTable] FOREIGN KEY([resfundType])
REFERENCES [dbo].[resfundTypeTable] ([resfundTypeID])
GO
ALTER TABLE [dbo].[resfundTable] CHECK CONSTRAINT [FK_resfundTable_resfundTypeTable]
GO
ALTER TABLE [dbo].[resfundTable]  WITH CHECK ADD  CONSTRAINT [FK_resfundTable_storeTable] FOREIGN KEY([store_id])
REFERENCES [dbo].[storeTable] ([storeID])
GO
ALTER TABLE [dbo].[resfundTable] CHECK CONSTRAINT [FK_resfundTable_storeTable]
GO
ALTER TABLE [dbo].[resfundTable]  WITH CHECK ADD  CONSTRAINT [FK_resfundTable_userTable] FOREIGN KEY([user_id])
REFERENCES [dbo].[userTable] ([userID])
GO
ALTER TABLE [dbo].[resfundTable] CHECK CONSTRAINT [FK_resfundTable_userTable]
GO
ALTER TABLE [dbo].[userTable]  WITH CHECK ADD  CONSTRAINT [FK_userTable_roleTable] FOREIGN KEY([role_id])
REFERENCES [dbo].[roleTable] ([roleID])
GO
ALTER TABLE [dbo].[userTable] CHECK CONSTRAINT [FK_userTable_roleTable]
GO
ALTER TABLE [dbo].[userTable]  WITH CHECK ADD  CONSTRAINT [FK_userTable_storeTable] FOREIGN KEY([store_id])
REFERENCES [dbo].[storeTable] ([storeID])
GO
ALTER TABLE [dbo].[userTable] CHECK CONSTRAINT [FK_userTable_storeTable]
GO
/****** Object:  StoredProcedure [dbo].[masrafRaporHazirlamaSP]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[masrafRaporHazirlamaSP]

@startdate date,
@enddate date
as
begin
select 
storeTable.storeDescription As 'Mağaza Adı'
,expenceTable.expenceCode As 'Masraf Kodu'
,invoiceTable.expenceDescription As 'Masraf Açıklama'
,SUM(amount) as 'Masraf'
from invoiceTable
LEFT OUTER JOIN  expenceTable on expenceTable.expenceID=invoiceTable.expence_id
LEFT OUTER JOIN storeTable on storeTable.storeID=invoiceTable.store_id

where invoiceDate BETWEEN @startdate AND  @enddate 

group by storeTable.storeDescription,expenceTable.expenceCode,invoiceTable.expenceDescription
end
GO
/****** Object:  StoredProcedure [dbo].[ozetMasrafRaporHazirlamaSP]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[ozetMasrafRaporHazirlamaSP]

@startdate date,
@enddate date,
@store_id int
as
begin
select 
storeTable.storeName As 'Mağaza Adı'
,expenceTable.expenceCode As 'Masraf Adı'
,invoiceTable.expenceDescription As 'Masraf Açıklama'
,SUM(amount) as 'Masraf'
from invoiceTable
LEFT OUTER JOIN  expenceTable on expenceTable.expenceID=invoiceTable.expence_id
LEFT OUTER JOIN storeTable on storeTable.storeID=invoiceTable.store_id

where store_id=@store_id and invoiceDate BETWEEN @startdate AND  @enddate

group by storeTable.storeName,invoiceTable.expenceDescription,expenceTable.expenceCode
end
GO
/****** Object:  StoredProcedure [dbo].[raporHazirlamaSP]    Script Date: 27.09.2022 12:44:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[raporHazirlamaSP]

@startdate date,
@enddate date
as
begin
select 
storeTable.storeDescription As 'Mağaza Adı'
,SUM(cash) AS 'Nakit'
,SUM(credit) as 'Banka/Kredi Kartı'
,SUM(amount) as 'Masraf'
,(SUM(cash)+Sum(credit))-(Sum(amount)) as 'Durum'
from invoiceTable
LEFT OUTER JOIN bankTable ON bankTable.bankID=invoiceTable.bank_id
LEFT OUTER JOIN  expenceTable on expenceTable.expenceID=invoiceTable.expence_id
LEFT OUTER JOIN storeTable on storeTable.storeID=invoiceTable.store_id

where invoiceDate BETWEEN @startdate AND  @enddate 

group by storeTable.storeDescription
end
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[45] 4[14] 2[19] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "invoiceLogTable"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 249
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "bankTable"
            Begin Extent = 
               Top = 7
               Left = 297
               Bottom = 170
               Right = 500
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "bankTable_1"
            Begin Extent = 
               Top = 7
               Left = 548
               Bottom = 170
               Right = 751
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "expenceTable"
            Begin Extent = 
               Top = 7
               Left = 799
               Bottom = 148
               Right = 1025
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "expenceTable_1"
            Begin Extent = 
               Top = 154
               Left = 799
               Bottom = 295
               Right = 1025
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "storeTable"
            Begin Extent = 
               Top = 175
               Left = 48
               Bottom = 338
               Right = 252
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "storeTable_1"
            Begin Extent = 
               Top = 175
               Left = 300
               Bottom = 338
               Right = 504
            End' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwInvoiceLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 30
         Width = 284
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
         Width = 1200
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1680
         Table = 1176
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwInvoiceLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwInvoiceLog'
GO
