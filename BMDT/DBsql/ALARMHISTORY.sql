/*
 Navicat Premium Data Transfer

 Source Server         : ECS
 Source Server Type    : SQLite
 Source Server Version : 3017000
 Source Schema         : main

 Target Server Type    : SQLite
 Target Server Version : 3017000
 File Encoding         : 65001

 Date: 17/07/2019 08:46:44
*/

PRAGMA foreign_keys = false;

-- ----------------------------
-- Table structure for ALARMHISTORY
-- ----------------------------
DROP TABLE IF EXISTS "ALARMHISTORY";
CREATE TABLE "ALARMHISTORY" (
  "OBJECTNO" Varchar(255) NOT NULL,
  "ALST" Integer,
  "ALCD" Integer,
  "ALID" Varchar(5),
  "UNITID" Varchar(20) NOT NULL,
  "ALTX" Varchar(40),
  "HISTORYTIME" DATETIME NOT NULL,
  "EVENTNAME" Varchar(40),
  PRIMARY KEY ("OBJECTNO")
);

PRAGMA foreign_keys = true;
