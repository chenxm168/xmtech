/*
 Navicat Premium Data Transfer

 Source Server         : ECS
 Source Server Type    : SQLite
 Source Server Version : 3017000
 Source Schema         : main

 Target Server Type    : SQLite
 Target Server Version : 3017000
 File Encoding         : 65001

 Date: 31/08/2019 08:32:53
*/

PRAGMA foreign_keys = false;

-- ----------------------------
-- Table structure for PORT
-- ----------------------------
DROP TABLE IF EXISTS "PORT";
CREATE TABLE "PORT" (
  "EQUIPMENTNAME" Varchar(10) NOT NULL,
  "PORTNAME" Varchar(20) NOT NULL,
  "BCREXIST" Integer,
  "CAPACITY" Integer,
  "CASSETTESEQNO" Integer,
  "CASSETTETYPE" Varchar(10),
  "CURRENTSTATUSTIME" DATETIME,
  "GRADEPRIORITY" Integer,
  "HIGHLIMIT" Integer,
  "LOCALNO" Integer NOT NULL,
  "LOWLIMIT" Integer,
  "MAPPINGUNITEXIST" Integer,
  "PORTENABLEMODE" Varchar(10),
  "PORTGRADE" Varchar(20),
  "PORTMODE" Varchar(20),
  "PORTNO" Integer NOT NULL,
  "PORTQTIME" Integer,
  "PORTSTATUS" Varchar(20),
  "PORTTRANSFERMODE" Varchar(20),
  "PORTTYPE" Varchar(20),
  "PORTTYPEAUTOMODE" Varchar(20),
  "PORTUSETYPE" Varchar(4),
  "CARRIERID"  Varchar(12),
  "DESCRIPTION" Varchar(255),
  PRIMARY KEY ("EQUIPMENTNAME", "PORTNAME")
);

-- ----------------------------
-- Records of "PORT"
-- ----------------------------
INSERT INTO "PORT" VALUES ('C1CCL01', 'PL01', 0, 0, 0, 'NORMAL', '2019-04-29 12:11:47.275', 0, 0, 2, 0, 0, 'Enable', '', 'TFT', 1, 10, 'UnloadRequest', 'StockerInline', 'PL', 'Disable', 'CF',' ', 'PORT#1');
INSERT INTO "PORT" VALUES ('C1CCL01', 'PU01', 0, 0, 0, 'NORMAL', '2019-04-29 10:53:45.401', 0, 0, 2, 0, 0, 'Enable', '', 'TFT', 1, 10, 'Reserved', 'StockerInline', 'PU', 'Disable', 'CF', ' ','PORT#2');

PRAGMA foreign_keys = true;
