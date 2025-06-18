-- Crear la base de datos (opcional, se puede crear desde la conexión)
-- CREATE DATABASE BookReviewDB;
-- GO

-- USAR la base de datos
-- USE BookReviewDB;
-- GO

-- Tablas de ASP.NET Core Identity (EF Core las generará, pero aquí está la estructura base)

CREATE TABLE AspNetRoles (
    Id NVARCHAR(450) NOT NULL PRIMARY KEY,
    Name NVARCHAR(256) NULL,
    NormalizedName NVARCHAR(256) NULL,
    ConcurrencyStamp NVARCHAR(MAX) NULL
);

CREATE TABLE AspNetUsers (
    Id NVARCHAR(450) NOT NULL PRIMARY KEY,
    UserName NVARCHAR(256) NULL,
    NormalizedUserName NVARCHAR(256) NULL,
    Email NVARCHAR(256) NULL,
    NormalizedEmail NVARCHAR(256) NULL,
    EmailConfirmed BIT NOT NULL,
    PasswordHash NVARCHAR(MAX) NULL,
    SecurityStamp NVARCHAR(MAX) NULL,
    ConcurrencyStamp NVARCHAR(MAX) NULL,
    PhoneNumber NVARCHAR(MAX) NULL,
    PhoneNumberConfirmed BIT NOT NULL,
    TwoFactorEnabled BIT NOT NULL,
    LockoutEnd DATETIMEOFFSET NULL,
    LockoutEnabled BIT NOT NULL,
    AccessFailedCount INT NOT NULL,
    ProfilePictureUrl NVARCHAR(MAX) NULL -- Campo personalizado para foto de perfil
);

CREATE TABLE AspNetRoleClaims (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    RoleId NVARCHAR(450) NOT NULL FOREIGN KEY REFERENCES AspNetRoles(Id) ON DELETE CASCADE,
    ClaimType NVARCHAR(MAX) NULL,
    ClaimValue NVARCHAR(MAX) NULL
);

CREATE TABLE AspNetUserClaims (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL FOREIGN KEY REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    ClaimType NVARCHAR(MAX) NULL,
    ClaimValue NVARCHAR(MAX) NULL
);

CREATE TABLE AspNetUserLogins (
    LoginProvider NVARCHAR(450) NOT NULL,
    ProviderKey NVARCHAR(450) NOT NULL,
    ProviderDisplayName NVARCHAR(MAX) NULL,
    UserId NVARCHAR(450) NOT NULL FOREIGN KEY REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    PRIMARY KEY (LoginProvider, ProviderKey)
);

CREATE TABLE AspNetUserRoles (
    UserId NVARCHAR(450) NOT NULL FOREIGN KEY REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    RoleId NVARCHAR(450) NOT NULL FOREIGN KEY REFERENCES AspNetRoles(Id) ON DELETE CASCADE,
    PRIMARY KEY (UserId, RoleId)
);

CREATE TABLE AspNetUserTokens (
    UserId NVARCHAR(450) NOT NULL FOREIGN KEY REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    LoginProvider NVARCHAR(450) NOT NULL,
    Name NVARCHAR(450) NOT NULL,
    Value NVARCHAR(MAX) NULL,
    PRIMARY KEY (UserId, LoginProvider, Name)
);

-- Tablas personalizadas de la aplicación

CREATE TABLE Categories (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Books (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(200) NOT NULL,
    Summary NVARCHAR(MAX) NOT NULL,
    CoverImageUrl NVARCHAR(MAX) NULL,
    CategoryId INT NOT NULL FOREIGN KEY REFERENCES Categories(Id)
);

CREATE TABLE Reviews (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Rating INT NOT NULL CHECK (Rating >= 1 AND Rating <= 5),
    Comment NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    BookId INT NOT NULL FOREIGN KEY REFERENCES Books(Id) ON DELETE CASCADE,
    UserId NVARCHAR(450) NOT NULL FOREIGN KEY REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

-- Índices para mejorar el rendimiento de las búsquedas
CREATE INDEX IX_AspNetUsers_NormalizedUserName ON AspNetUsers(NormalizedUserName);
CREATE INDEX IX_AspNetUsers_NormalizedEmail ON AspNetUsers(NormalizedEmail);
CREATE INDEX IX_Books_Title ON Books(Title);
CREATE INDEX IX_Books_Author ON Books(Author);
CREATE INDEX IX_Reviews_BookId ON Reviews(BookId);
CREATE INDEX IX_Reviews_UserId ON Reviews(UserId);


-- SEED DATA (Datos de ejemplo)

INSERT INTO Categories (Name) VALUES
('Ficción'),
('Ciencia Ficción'),
('Misterio'),
('Fantasía'),
('No Ficción'),
('Biografía');

INSERT INTO Books (Title, Author, Summary, CategoryId, CoverImageUrl) VALUES
('Cien años de soledad', 'Gabriel García Márquez', 'La historia de la familia Buendía a lo largo de siete generaciones en el pueblo ficticio de Macondo.', 1, 'https://images.penguinrandomhouse.com/cover/9780307474728'),
('Dune', 'Frank Herbert', 'La historia de Paul Atreides, un joven brillante destinado a un gran propósito, que debe viajar al planeta más peligroso del universo para asegurar el futuro de su familia y su pueblo.', 2, 'https://images.penguinrandomhouse.com/cover/9780441013593'),
('El código Da Vinci', 'Dan Brown', 'El experto en simbología Robert Langdon se ve envuelto en una búsqueda del tesoro a través de Europa para descubrir un antiguo secreto protegido por una sociedad secreta.', 3, 'https://images.penguinrandomhouse.com/cover/9780307474278'),
('El Señor de los Anillos', 'J.R.R. Tolkien', 'Un hobbit llamado Frodo Bolsón se embarca en una peligrosa misión para destruir un poderoso anillo y salvar a la Tierra Media de la oscuridad.', 4, 'https://images.penguinrandomhouse.com/cover/9780618640157');

GO