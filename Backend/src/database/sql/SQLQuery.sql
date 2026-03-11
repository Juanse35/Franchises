/* this SQL script creates the necessary tables for the franchise management application */
/*use db_Franchise;

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

CREATE TABLE tbl_product (
    id_product INT IDENTITY(1,1) PRIMARY KEY,
    name_product NVARCHAR(150) NOT NULL,
    branch_id INT NOT NULL,
    stock INT NOT NULL DEFAULT 0,
    registration_date DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_product_branch FOREIGN KEY (branch_id) REFERENCES tbl_branch(id_branch)
);*/

select * from tbl_product;
SELECT 
    p.id_product,
    p.name_product,
    p.stock,
    p.registration_date AS product_registration,
    b.name_branch AS branch_name,
    f.name AS franchise_name
FROM tbl_product p
INNER JOIN tbl_branch b ON p.branch_id = b.id_branch
INNER JOIN tbl_franchise f ON b.franchise_id = f.id;


