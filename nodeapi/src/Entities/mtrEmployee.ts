/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {mtrEmployeeInstance, mtrEmployeeAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<mtrEmployeeInstance, mtrEmployeeAttribute>('mtrEmployee', {
    employeeID: {
      type: DataTypes.BIGINT,
      allowNull: false,
      defaultValue: '((0))',
      primaryKey: true
    },
    employeeCode: {
      type: DataTypes.STRING,
      allowNull: true
    },
    employeeName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    lid: {
      type: DataTypes.BIGINT,
      allowNull: true,
      defaultValue: '((0))'
    },
    address: {
      type: DataTypes.STRING,
      allowNull: true
    },
    contactNo: {
      type: DataTypes.STRING,
      allowNull: true
    },
    gender: {
      type: DataTypes.STRING,
      allowNull: true
    },
    bloodGroup: {
      type: DataTypes.STRING,
      allowNull: true
    },
    startTime: {
      type: DataTypes.STRING,
      allowNull: true
    },
    endTime: {
      type: DataTypes.STRING,
      allowNull: true
    },
    salary: {
      type: DataTypes.FLOAT,
      allowNull: true,
      defaultValue: '((0))'
    },
    deactive: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    categoryID: {
      type: DataTypes.INTEGER,
      allowNull: true,
      defaultValue: '((0))'
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    },
    commPer: {
      type: DataTypes.FLOAT,
      allowNull: true
    }
  }, {
    tableName: 'mtrEmployee'
  });
};
