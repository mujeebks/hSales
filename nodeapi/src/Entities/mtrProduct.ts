/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {mtrProductInstance, mtrProductAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<mtrProductInstance, mtrProductAttribute>('mtrProduct', {
    prodID: {
      type: DataTypes.BIGINT,
      allowNull: false,
      primaryKey: true
    },
    prodCode: {
      type: DataTypes.STRING,
      allowNull: true
    },
    prodName: {
      type: DataTypes.STRING,
      allowNull: false
    },
    categoryid: {
      type: DataTypes.BIGINT,
      allowNull: false
    },
    sTax: {
      type: "MONEY",
      allowNull: true,
      defaultValue: '((0))'
    },
    pTax: {
      type: "MONEY",
      allowNull: true,
      defaultValue: '((0))'
    },
    staxincl: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    ptaxincl: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    mnf: {
      type: DataTypes.STRING,
      allowNull: true
    },
    mainUnit: {
      type: DataTypes.STRING,
      allowNull: true
    },
    Rack: {
      type: DataTypes.STRING,
      allowNull: true
    },
    prate: {
      type: "MONEY",
      allowNull: true
    },
    srate: {
      type: "MONEY",
      allowNull: true
    },
    mrp: {
      type: "MONEY",
      allowNull: true
    },
    active: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    igstPer: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    rol: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    remarks: {
      type: DataTypes.STRING,
      allowNull: true
    },
    prateper: {
      type: DataTypes.FLOAT,
      allowNull: true,
      defaultValue: '((0))'
    },
    srateper: {
      type: DataTypes.FLOAT,
      allowNull: true,
      defaultValue: '((0))'
    },
    regionalName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    blnExpiry: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    },
    alternateUnit: {
      type: DataTypes.STRING,
      allowNull: true
    },
    convfactor: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    itemClassID: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    Batch: {
      type: DataTypes.STRING,
      allowNull: true
    },
    hsnCode: {
      type: DataTypes.STRING,
      allowNull: true
    },
    IsBoardItem: {
      type: DataTypes.BOOLEAN,
      allowNull: true
    },
    pluCode: {
      type: DataTypes.STRING,
      allowNull: true
    },
    cessPer: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    AddCess: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    OSRate: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    outerBatch: {
      type: DataTypes.STRING,
      allowNull: true
    },
    SPComPer: {
      type: DataTypes.FLOAT,
      allowNull: true
    }
  }, {
    tableName: 'mtrProduct'
  });
};
