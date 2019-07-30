/*
 Navicat Premium Data Transfer

 Source Server         : ECS
 Source Server Type    : SQLite
 Source Server Version : 3017000
 Source Schema         : main

 Target Server Type    : SQLite
 Target Server Version : 3017000
 File Encoding         : 65001

 Date: 16/07/2019 16:46:46
*/

PRAGMA foreign_keys = false;

-- ----------------------------
-- Table structure for MACHINEHISTORY
-- ----------------------------
DROP TABLE IF EXISTS "MACHINEHISTORY";
CREATE TABLE "MACHINEHISTORY" (
  "OBJECTNO" Varchar (255) NOT NULL,
  "CAPACITY" Integer,
  "CIMMODE" Varchar (5),
  "COMMTYPE" Integer,
  "CURRENTSTATUSTIME" DATETIME,
  "HIGHLIMIT" Integer,
  "LOCALNO" Integer NOT NULL,
  "LOWLIMIT" Integer,
  "MACHINEALIVE" Varchar (10) DEFAULT 'Down',
  "MACHINEAUTOMODE" Varchar (5),
  "MACHINEMODE" Varchar (15),
  "MACHINESTATUS" Varchar (10),
  "MACHINETYPE" Varchar (10),
  "OLDMACHINESTATUS" Varchar (10),
  "ONLINECONTROLSTATUS" Varchar (7),
  "PMCODE" Varchar (255) DEFAULT 'NotUsed',
  "RECIPEFIGURENUMBER" Integer DEFAULT 2,
  "RECIPENAME" Varchar (10),
  "STATUSCODE" Varchar (10),
  "EVENTNAME" Varchar (40) NOT NULL,
  "HISTORYTIME" DATETIME NOT NULL,
  "MACHINENAME" Varchar (20) NOT NULL,
  "EQUIPMENTNAME" Varchar (10) NOT NULL,
  "DESCRIPTION" Varchar (255),
  PRIMARY KEY ("OBJECTNO")
);

PRAGMA foreign_keys = true;
