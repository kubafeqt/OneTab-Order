CREATE TABLE [dbo].[BrowserOneTab] (
	[BrowserId]			 INT            IDENTITY (1, 1) NOT NULL,
    [BrowserName]        VARCHAR (128)  NOT NULL,
    [OneTabUrl]          NVARCHAR (512) NULL,	
	 CONSTRAINT [PK_BrowserOneTab] PRIMARY KEY CLUSTERED ([BrowserId] ASC),
);