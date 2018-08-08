/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {CompanyInstance, CompanyAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<CompanyInstance, CompanyAttribute>('Company', {
    companyid: {
      type: DataTypes.INTEGER,
      allowNull: false,
      defaultValue: '((0))',
      primaryKey: true
    },
    cdbName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    companyName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    blnHide: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    productKey: {
      type: DataTypes.STRING,
      allowNull: true
    },
    licenseKey: {
      type: DataTypes.STRING,
      allowNull: true
    },
    registered: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    expired: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    },
    AppID: {
      type: DataTypes.INTEGER,
      allowNull: true
    }
  }, {
    tableName: 'Company'
  });
};
