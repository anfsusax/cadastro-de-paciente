    USE Be3DB;
    GO

    IF NOT EXISTS (SELECT 1 FROM dbo.Convenios)
    BEGIN
        INSERT INTO dbo.Convenios (Nome, Ativo) VALUES
        ('Unimed', 1),
        ('SulAmérica', 1),
        ('Bradesco Saúde', 1),
        ('Amil', 1),
        ('NotreDame Intermédica', 1),
        ('Golden Cross', 1),
        ('Medial Saúde', 1);
    END
    GO
