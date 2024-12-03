-- chạy dòng ni trước
create database PBL4;



--Sau mới chạy các dòng sau được. Có database rồi thì các dòng sau chạy chung một lượt

use PBL4

create table Vai_Tro
(
	ID tinyint not null 
	constraint PK__Vai_Tro__ID primary key,
	Ten_Vai_Tro nvarchar(20) not null 
	constraint UK__Vai_Tro__Ten_Vai_Tro unique,
)

create table Tai_Khoan
(
	ID int not null 
	constraint PK__Tai_Khoan__ID primary key, 
	Ten_Dang_Nhap varchar(20) not null 
	constraint UK__Tai_Khoan__Ten_Dang_Nhap unique,
	Mat_Khau varchar(20) not null,
	Ho_Ten nvarchar(30) not null, 
	ID_Vai_Tro tinyint not null
	constraint FK__Tai_Khoan__ID_Vai_Tro__Vai_Tro__ID
	foreign key references Vai_Tro(ID),
	So_Du decimal(9, 2) not null,
)

create table Giao_Dich
(
	ID int not null constraint PK__Giao_Dich__ID primary key,
	ID_Tai_Khoan int not null
	constraint FK__Giao_Dich__ID_Tai_Khoan__Tai_Khoan__ID
	foreign key references Tai_Khoan(ID),
	Thoi_Gian datetime2(0) not null,
	So_Tien decimal(9, 2) not null,
	Noi_Dung_Giao_Dich text not null,
)

create table Kho_Hang
(
	ID tinyint not null constraint PK__Kho_Hang__ID primary key,
	Ten_Hang nvarchar(30) not null constraint UK__Kho_Hang__Ten_Hang unique,
	So_Luong int not null,
)

create table Phan_Loai_Hang
(
	ID tinyint not null constraint PK__Phan_Loai_Hang__ID primary key,
	Ten_Loai nvarchar(30) not null constraint UK__Kho_Hang__Ten_Loai unique,
)

create table Mat_Hang
(
	ID tinyint not null constraint PK__Mat_Hang__ID primary key,
	Ten_Mat_Hang nvarchar(30) not null constraint UK__Mat_Hang__Ten_Mat_Hang unique,
	Gia int not null,
	Mo_Ta text not null,
	URL_Hinh_Anh text null,
	ID_Phan_Loai_Hang tinyint not null
	constraint FK__Mat_Hang__ID_Phan_Loai_Hang__Phan_Loai_Hang__ID
	foreign key references Phan_Loai_Hang(ID)
)

create table Loai_May
(
	ID tinyint not null constraint PK__Loai_May__ID primary key,
	Ten_Loai_May nvarchar(20) not null constraint UK__Loai_May__Ten_Loai_May unique,
)

create table Tinh_Trang_May
(
	ID tinyint not null constraint PK__Tinh_Trang_May__ID primary key,
	Ten_Tinh_Trang nvarchar(20) not null constraint UK__Tinh_Trang_May__Ten_Tinh_Trang unique,
)

create table May
(
	ID smallint not null constraint PK__May__ID primary key,
	Ten_May varchar(20) not null constraint UK__May__Ten_May unique,
	Dia_Chi_IPv4 varchar(15) not null constraint UK__May__Dia_Chi_IPv4 unique,
	Dia_Chi_MAC char(17) not null constraint UK__May__Dia_Chi_MAC unique,
	ID_Tinh_Trang tinyint not null
	constraint FK__May__ID_Tinh_Trang__Tinh_Trang_May__ID
	foreign key references Tinh_Trang_May(ID),
	ID_Loai_May tinyint not null
	constraint FK__May__ID_Loai_May__Loai_May__ID
	foreign key references Loai_May(ID),
)

create table Dang_Hoat_Dong
(
	ID_Tai_Khoan int not null 
	constraint FK__Dang_Hoat_Dong__ID_Tai_Khoan__Tai_Khoan__ID
	foreign key references Tai_Khoan(ID)
	constraint UK__Dang_Hoat_Dong__ID_Tai_Khoan unique,
	ID_May smallint not null 
	constraint FK__Dang_Hoat_Dong__ID_May__May__ID
	foreign key references May(ID)
	constraint UK__Dang_Hoat_Dong__ID_May unique,
	constraint PK__Dang_Hoat_Dong__ID_Tai_Khoan__ID_May primary key (ID_Tai_Khoan,ID_May)
)

create table Phan_Mem
(
	ID tinyint not null constraint PK__Phan_Mem__ID primary key,
	Ten_Phan_Mem nvarchar(255) constraint UK__Phan_Mem__Ten_Phan_Mem unique,
	Duong_Dan text not null,
	Dung_Luong bigint not null,
	Mo_Ta text null,
)

insert into Tinh_Trang_May
values (0, N'Đã tắt'), (1, N'Đã đăng nhập'), (2, N'Chưa đăng nhập')

insert into Vai_Tro
values (0, N'Admin'), (1, N'Khách hàng')

insert into Loai_May
values (0, N'Máy thường')
