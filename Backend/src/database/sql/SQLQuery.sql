/* this SQL script creates the necessary tables for the franchise management application */
use db_Franchise;

CREATE TABLE tbl_franchise (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(200) NOT NULL,
    registration_date DATETIME NOT NULL DEFAULT GETDATE(),
);

CREATE TABLE tbl_branch (
    id_branch INT IDENTITY(1,1) PRIMARY KEY,
    name_branch NVARCHAR(150) NOT NULL,
    franchise_id INT NOT NULL,
    registration_date DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_branch_franchise FOREIGN KEY (franchise_id)REFERENCES tbl_franchise(id)
);

CREATE TABLE tbl_product(
    id_product INT IDENTITY(1,1) PRIMARY KEY,
    name_product NVARCHAR(150) NOT NULL,
    imag_url NVARCHAR(500) NULL,
    branch_id INT NOT NULL,
    registration_date DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_product_branch FOREIGN KEY (branch_id) REFERENCES tbl_branch(id_branch)
);

select * from tbl_product;



