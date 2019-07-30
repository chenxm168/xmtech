/*
 Navicat Premium Data Transfer

 Source Server         : ECS
 Source Server Type    : SQLite
 Source Server Version : 3017000
 Source Schema         : main

 Target Server Type    : SQLite
 Target Server Version : 3017000
 File Encoding         : 65001

 Date: 17/07/2019 08:44:20
*/

PRAGMA foreign_keys = false;

-- ----------------------------
-- Table structure for MACHINE
-- ----------------------------
DROP TABLE IF EXISTS "MACHINE";
CREATE TABLE "MACHINE" (
  "EQUIPMENTNAME" Varchar(10) NOT NULL,
  "MACHINENAME" Varchar(20) NOT NULL,
  "CAPACITY" Integer,
  "CIMMODE" Varchar(5),
  "COMMTYPE" Integer,
  "CURRENTSTATUSTIME" DATETIME,
  "HIGHLIMIT" Integer,
  "LOCALNO" Integer NOT NULL,
  "LOWLIMIT" Integer,
  "MACHINEALIVE" Varchar(10) DEFAULT 'Down',
  "MACHINEAUTOMODE" Varchar(5),
  "MACHINEMODE" Varchar(15),
  "MACHINESTATUS" Varchar(10),
  "MACHINETYPE" Varchar(10),
  "OLDMACHINESTATUS" Varchar(10),
  "ONLINECONTROLSTATUS" Varchar(7),
  "PMCODE" Varchar(255) DEFAULT 'NotUsed',
  "RECIPEFIGURENUMBER" Integer DEFAULT 2,
  "RECIPENAME" Varchar(10),
  "STATUSCODE" Varchar(10),
  "DESCRIPTION" Varchar(255),
  PRIMARY KEY ("EQUIPMENTNAME", "MACHINENAME")
);

PRAGMA foreign_keys = true;
