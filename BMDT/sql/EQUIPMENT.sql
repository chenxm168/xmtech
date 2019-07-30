/*
 Navicat Premium Data Transfer

 Source Server         : BMDT
 Source Server Type    : SQLite
 Source Server Version : 3017000
 Source Schema         : main

 Target Server Type    : SQLite
 Target Server Version : 3017000
 File Encoding         : 65001

 Date: 25/07/2019 10:12:16
*/

PRAGMA foreign_keys = false;

-- ----------------------------
-- Table structure for EQUIPMENT
-- ----------------------------
DROP TABLE IF EXISTS "EQUIPMENT";
CREATE TABLE "EQUIPMENT" (
  "EQUIPMENTNAME" Varchar(10) NOT NULL,
  "RMSUSEFLAG" Varchar(5),
  "CURRENTSTATUSTIME" DATETIME,
  "EQUIPMENTSTATUS" Varchar(10),
  "EQUIPMENTTYPE" Varchar(30) NOT NULL,
  "OLDEQUIPMENTSTATUS" Varchar(10),
  "ONLINECONTROLSTATUS" Varchar(10),
  "PROJECTTYPE" Varchar(10),
  "SHOP" Varchar(5) NOT NULL,
  "STATUSCODE" Varchar(10),
  "DESCRIPTION" Varchar(255),
  PRIMARY KEY ("EQUIPMENTNAME")
);

-- ----------------------------
-- Records of "EQUIPMENT"
-- ----------------------------
INSERT INTO "EQUIPMENT" VALUES ('C1CCL01', 'N', '2019-04-24 16:02:47.924', 'Run', 'Cassette Cleaner', 'Run', 'Offline', 'New', 'Array', '', 'Cassette Cleaner');

PRAGMA foreign_keys = true;
