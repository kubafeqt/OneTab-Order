CREATE TABLE [dbo].[BrowserOneTab] (
    [BrowserName]       VARCHAR (128)  NOT NULL,
    [OneTabUrl]         NVARCHAR (512) NULL,
    [SampleHash]        VARCHAR (64)   NOT NULL,
    [ScreenStartX]      INT            NOT NULL,
    [ScreenStartY]      INT            NOT NULL,
    [RecognitionStartX] INT            NULL,
    [RecognitionStartY] INT            NULL
);

