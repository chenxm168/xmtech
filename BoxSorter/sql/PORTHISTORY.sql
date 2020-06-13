/*
 Navicat Premium Data Transfer

 Source Server         : ECS
 Source Server Type    : SQLite
 Source Server Version : 3017000
 Source Schema         : main

 Target Server Type    : SQLite
 Target Server Version : 3017000
 File Encoding         : 65001

 Date: 31/08/2019 08:33:05
*/

PRAGMA foreign_keys = false;

-- ----------------------------
-- Table structure for PORTHISTORY
-- ----------------------------
DROP TABLE IF EXISTS "PORTHISTORY";
CREATE TABLE "PORTHISTORY" (
  "OBJECTNO" Varchar(255) NOT NULL,
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
  "PORTUSETYPE" Varchar(2),
  "EVENTNAME" Varchar(40) NOT NULL,
  "HISTORYTIME" DATETIME NOT NULL,
  "MACHINENAME" Varchar(20) NOT NULL,
  "PORTNAME" Varchar(20) NOT NULL,
  "CARRIERID"  Varchar(12),
  "DESCRIPTION" Varchar(255),
  PRIMARY KEY ("OBJECTNO")
);

PRAGMA foreign_keys = true;
