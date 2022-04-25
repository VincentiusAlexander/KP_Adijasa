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
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4;

/*Data for the table `data_abu` */

insert  into `data_abu`(`id`,`nama_abu`,`nama_alternatif_abu`,`alamat_abu`,`jenis_kelamin`,`tanggal_lahir`,`tanggal_wafat`,`tanggal_kremasi`,`keterangan`) values 
(22,'','','','Laki-laki','2022-04-02 00:00:00','2022-03-31 00:00:00','2022-04-01 00:00:00',''),
(23,'a','','a','Laki-laki','2022-04-01 00:00:00','2022-04-08 00:00:00','2022-03-31 00:00:00',''),
(24,'a','','a','Laki-laki','2022-04-01 00:00:00','2022-04-08 00:00:00','2022-03-31 00:00:00',''),
(25,'a','','a','Laki-laki','2022-04-14 00:00:00','2022-04-16 00:00:00','2022-04-08 00:00:00',''),
(26,'q','','q','Laki-laki','2022-04-06 00:00:00','2022-03-31 00:00:00','2022-04-01 00:00:00',''),
(27,'q','','q','Laki-laki','2022-03-27 00:00:00','2022-03-31 00:00:00','2022-04-01 00:00:00',''),
(28,'p','p','p','Laki-laki','2022-04-15 00:00:00','2022-04-20 00:00:00','2022-04-01 00:00:00','p'),
(29,'r','','r','Laki-laki','2022-04-09 00:00:00','2022-04-08 00:00:00','2022-04-04 00:00:00','');

/*Table structure for table `kategori` */

DROP TABLE IF EXISTS `kategori`;

CREATE TABLE `kategori` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `nama` varchar(150) NOT NULL,
  `harga` int(255) NOT NULL,
  `status` int(1) DEFAULT 0 COMMENT '0 == Not Deleted, 1 == Deleted',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;

/*Data for the table `kategori` */

insert  into `kategori`(`id`,`nama`,`harga`,`status`) values 
(5,'Besar',10000,0),
(6,'kecil',5000,0);

/*Table structure for table `kotak` */

DROP TABLE IF EXISTS `kotak`;

CREATE TABLE `kotak` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `kategori_id` int(255) NOT NULL,
  `no_kotak` varchar(100) NOT NULL,
  `status` int(1) DEFAULT 0 COMMENT '0 == Not Deleted, 1 == Deleted',
  PRIMARY KEY (`id`),
  KEY `kategori_id` (`kategori_id`),
  CONSTRAINT `kotak_ibfk_1` FOREIGN KEY (`kategori_id`) REFERENCES `kategori` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;

/*Data for the table `kotak` */

insert  into `kotak`(`id`,`kategori_id`,`no_kotak`,`status`) values 
(8,5,'A1',0),
(9,6,'A2',0);

/*Table structure for table `penanggung_jawab` */

DROP TABLE IF EXISTS `penanggung_jawab`;

CREATE TABLE `penanggung_jawab` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `nama` varchar(255) NOT NULL,
  `alamat` varchar(255) NOT NULL,
  `nomor_telp` varchar(55) NOT NULL,
  `relasi` varchar(150) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4;

/*Data for the table `penanggung_jawab` */

insert  into `penanggung_jawab`(`id`,`nama`,`alamat`,`nomor_telp`,`relasi`) values 
(7,'b','b','b','b'),
(8,'b','b','b','b'),
(9,'b','b','b','b'),
(10,'w','w','w','w'),
(11,'w','w','w','w'),
(12,'o','o','o','o'),
(13,'i','i','i','i'),
(14,'t','t','t','t'),
(15,'y','y','y','y');

/*Table structure for table `penitipan` */

DROP TABLE IF EXISTS `penitipan`;

CREATE TABLE `penitipan` (
  `id` int(255) NOT NULL AUTO_INCREMENT,
  `tanggal_registrasi` datetime NOT NULL,
  `tanggal_titip` datetime DEFAULT NULL,
  `tanggal_ambil` datetime DEFAULT NULL,
  `kotak_id` int(255) NOT NULL,
  `data_abu_id` int(11) NOT NULL,
  `penanggung_jawab_satu_id` int(11) NOT NULL,
  `penanggung_jawab_dua_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id_kotak` (`kotak_id`),
  CONSTRAINT `penitipan_ibfk_1` FOREIGN KEY (`kotak_id`) REFERENCES `kotak` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;

/*Data for the table `penitipan` */

insert  into `penitipan`(`id`,`tanggal_registrasi`,`tanggal_titip`,`tanggal_ambil`,`kotak_id`,`data_abu_id`,`penanggung_jawab_satu_id`,`penanggung_jawab_dua_id`) values 
(1,'2022-04-25 18:13:00','2022-04-08 00:00:00','2022-04-01 00:00:00',8,28,12,-1),
(2,'2022-04-25 18:16:44','2022-04-09 00:00:00','2022-03-31 00:00:00',8,29,14,15);

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
(2,'test','098f6bcd4621d373cade4e832627b4f6',1);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
