CREATE TABLE [dbo].[ImageRecognition] (
		-- Vlastní ID pro tento řádek (doporučeno pro správu dat)
    [RecognitionId]     INT           IDENTITY (1, 1) NOT NULL, 
    -- Cizí klíč ukazující na prohlížeč
    [BrowserId]         INT           NOT NULL,
	 [ConfigName]        NVARCHAR(128) NOT NULL, -- Např. '4K', 'Notebook', 'Full HD'
    [SampleHash]        VARCHAR (64)  NOT NULL,
    [ScreenStartX]      INT           NOT NULL,
    [ScreenStartY]      INT           NOT NULL,
    [ScreenWidth]       INT           NOT NULL,
    [ScreenHeight]      INT           NOT NULL,
    [RecognitionStartX] INT           NULL,
    [RecognitionStartY] INT           NULL,
     -- Primární klíč této tabulky
    CONSTRAINT [PK_ImageRecognition] PRIMARY KEY CLUSTERED ([RecognitionId] ASC),
    
    -- Vaše správná vazba
    CONSTRAINT [FK_ImageRecognition_BrowserOneTab] FOREIGN KEY ([BrowserId]) 
        REFERENCES [dbo].[BrowserOneTab] ([BrowserId])
        ON DELETE CASCADE -- (Volitelné: když smažete Chrome, smažou se i jeho recognition data)
);