/*
SQLyog Community v13.1.7 (64 bit)
MySQL - 10.4.21-MariaDB : Database - penitipan_abu_adijasa
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`penitipan_abu_adijasa` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;

USE `penitipan_abu_adijasa`;

/*Table structure for table `data_abu` */

DROP TABLE IF EXISTS `data_abu`;

CREATE TABLE `data_abu` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `nama_abu` varchar(255) NOT NULL,
  `nama_alternatif_abu` varchar(255) DEFAULT NULL,
  `alamat_abu` varchar(255) NOT NULL,
  `jenis_kelamin` varchar(25) NOT NULL,
  `tanggal_lahir` datetime NOT NULL,
  `tanggal_wafat` datetime NOT NULL,
  `tanggal_kremasi` datetime NOT NULL,
  `keterangan` longtext DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*Data for the table `data_abu` */

insert  into `data_abu`(`id`,`nama_abu`,`nama_alternatif_abu`,`alamat_abu`,`jenis_kelamin`,`tanggal_lahir`,`tanggal_wafat`,`tanggal_kremasi`,`keterangan`) values 
(1,'Hansen','Hansen','asd','Laki-laki','2000-03-08 00:00:00','2022-04-25 00:00:00','2022-04-25 00:00:00','ads'),
(2,'asd','asd','asd','Laki-laki','2022-04-25 00:00:00','2022-04-25 00:00:00','2022-04-25 00:00:00','asd');

/*Table structure for table `kategori` */

DROP TABLE IF EXISTS `kategori`;

CREATE TABLE `kategori` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `nama` varchar(150) NOT NULL,
  `harga` int(255) NOT NULL,
  `status` int(1) DEFAULT 0 COMMENT '0 == Not Deleted, 1 == Deleted',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*Data for the table `kategori` */

insert  into `kategori`(`id`,`nama`,`harga`,`status`) values 
(1,'Besar',15000,0),
(2,'Kecil',12000,0);

/*Table structure for table `kotak` */

DROP TABLE IF EXISTS `kotak`;

CREATE TABLE `kotak` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `kategori_id` int(255) NOT NULL,
  `no_kotak` varchar(100) NOT NULL,
  `status` int(1) DEFAULT 0 COMMENT '0 == Not Deleted, 1 == Deleted',
  `terpakai` int(1) DEFAULT 0,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4;

/*Data for the table `kotak` */

insert  into `kotak`(`id`,`kategori_id`,`no_kotak`,`status`,`terpakai`) values 
(1,1,'A1',0,0),
(2,1,'A2',0,0),
(3,1,'A3',0,0),
(4,1,'A4',0,0),
(5,1,'A5',0,0),
(6,1,'A6',0,0),
(7,2,'B1',0,0),
(8,1,'B2',0,0),
(9,1,'B3',0,0),
(10,1,'B4',0,0),
(11,1,'B5',0,0),
(12,1,'B6',0,0);

/*Table structure for table `pembayaran_sewa` */

DROP TABLE IF EXISTS `pembayaran_sewa`;

CREATE TABLE `pembayaran_sewa` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_penitipan` int(11) NOT NULL,
  `id_kotak` int(11) NOT NULL,
  `harga_kotak` int(11) NOT NULL,
  `harga_total_sewa` int(11) NOT NULL,
  `tanggal_awal` datetime NOT NULL,
  `tanggal_akhir` datetime NOT NULL,
  `status` int(1) NOT NULL DEFAULT 0,
  `tanggal_diambil` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id_penitipan` (`id_penitipan`),
  KEY `id_kotak` (`id_kotak`),
  CONSTRAINT `pembayaran_sewa_ibfk_1` FOREIGN KEY (`id_penitipan`) REFERENCES `penitipan` (`id`),
  CONSTRAINT `pembayaran_sewa_ibfk_2` FOREIGN KEY (`id_kotak`) REFERENCES `kotak` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*Data for the table `pembayaran_sewa` */

insert  into `pembayaran_sewa`(`id`,`id_penitipan`,`id_kotak`,`harga_kotak`,`harga_total_sewa`,`tanggal_awal`,`tanggal_akhir`,`status`,`tanggal_diambil`) values 
(1,1,8,15000,75000,'2022-04-26 00:00:00','2022-08-27 00:00:00',0,NULL),
(2,2,3,15000,15000,'2022-04-27 00:00:00','2022-04-27 00:00:00',0,NULL);

/*Table structure for table `penanggung_jawab` */

DROP TABLE IF EXISTS `penanggung_jawab`;

CREATE TABLE `penanggung_jawab` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `nama` varchar(255) NOT NULL,
  `alamat` varchar(255) NOT NULL,
  `nomor_telp` varchar(55) NOT NULL,
  `relasi` varchar(150) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

/*Data for the table `penanggung_jawab` */

insert  into `penanggung_jawab`(`id`,`nama`,`alamat`,`nomor_telp`,`relasi`) values 
(1,'Mulya','ddd','2we13','Cek'),
(2,'Chandra','ddas','2we13','Cek'),
(3,'asd','asd','asd','asd');

/*Table structure for table `pengambilan_abu` */

DROP TABLE IF EXISTS `pengambilan_abu`;

CREATE TABLE `pengambilan_abu` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_penitipan` int(11) NOT NULL,
  `tanggal_pengambilan` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

/*Data for the table `pengambilan_abu` */

/*Table structure for table `penitipan` */

DROP TABLE IF EXISTS `penitipan`;

CREATE TABLE `penitipan` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `tanggal_registrasi` date NOT NULL,
  `tanggal_titip` date DEFAULT NULL,
  `tanggal_ambil` date DEFAULT NULL,
  `kotak_id` int(255) NOT NULL,
  `data_abu_id` int(11) NOT NULL,
  `penanggung_jawab_satu_id` int(11) NOT NULL,
  `penanggung_jawab_dua_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*Data for the table `penitipan` */

insert  into `penitipan`(`id`,`tanggal_registrasi`,`tanggal_titip`,`tanggal_ambil`,`kotak_id`,`data_abu_id`,`penanggung_jawab_satu_id`,`penanggung_jawab_dua_id`) values 
(1,'2022-04-25','2022-04-26','2027-04-26',8,1,1,2),
(2,'2022-04-25','2022-04-25','2022-04-25',3,2,3,-1);

/*Table structure for table `users` */

DROP TABLE IF EXISTS `users`;

CREATE TABLE `users` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `role` int(1) NOT NULL COMMENT '1 = admin, 0 = staff',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*Data for the table `users` */

insert  into `users`(`id`,`username`,`password`,`role`) values 
(1,'test','098f6bcd4621d373cade4e832627b4f6',0),
(2,'admin','21232f297a57a5a743894a0e4a801fc3',1);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
