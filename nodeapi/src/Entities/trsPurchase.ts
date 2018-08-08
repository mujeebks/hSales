/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsPurchaseInstance, trsPurchaseAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsPurchaseInstance, trsPurchaseAttribute>('trsPurchase', {
    invid: {
      type: DataTypes.BIGINT,
      allowNull: false,
      primaryKey: true
    },
    invType: {
      type: DataTypes.STRING,
      allowNull: true
    },
    invno: {
      type: DataTypes.STRING,
      allowNull: false
    },
    invdate: {
      type: DataTypes.DATE,
      allowNull: true
    },
    party: {
      type: DataTypes.STRING,
      allowNull: true
    },
    lid: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    mop: {
      type: DataTypes.STRING,
      allowNull: true
    },
    taxType: {
      type: DataTypes.STRING,
      allowNull: true
    },
    contactno: {
      type: DataTypes.STRING,
      allowNull: true
    },
    remarks: {
      type: DataTypes.STRING,
      allowNull: true
    },
    address: {
      type: DataTypes.STRING,
      allowNull: true
    },
    SalePersonID: {
      type: DataTypes.INTEGER,
      allowNull: true,
      defaultValue: '((0))'
    },
    grossAmount: {
      type: "MONEY",
      allowNull: true,
      defaultValue: '((0))'
    },
    Discount: {
      type: "MONEY",
      allowNull: true,
      defaultValue: '((0))'
    },
    taxableAmount: {
      type: "MONEY",
      allowNull: true,
      defaultValue: '((0))'
    },
    taxAmount: {
      type: "MONEY",
      allowNull: true,
      defaultValue: '((0))'
    },
    Exempted: {
      type: "MONEY",
      allowNull: true,
      defaultValue: '((0))'
    },
    roundOff: {
      type: "MONEY",
      allowNull: true,
      defaultValue: '((0))'
    },
    cashDiscount: {
      type: "MONEY",
      allowNull: true,
      defaultValue: '((0))'
    },
    billAmount: {
      type: "MONEY",
      allowNull: true
    },
    roundOffMode: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    otherExpense: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    freightCoolie: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    referenceNo: {
      type: DataTypes.STRING,
      allowNull: true
    },
    cancelled: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    blnOrder: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    OrderID: {
      type: DataTypes.BIGINT,
      allowNull: true,
      defaultValue: '((0))'
    },
    tinNo: {
      type: DataTypes.STRING,
      allowNull: true
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    },
    costfactor: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    compName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    userid: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    OrderDetail: {
      type: DataTypes.STRING,
      allowNull: true
    },
    DespatchDetail: {
      type: DataTypes.STRING,
      allowNull: true
    },
    cessAmount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    AddCessAmt: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    rndCutoff: {
      type: DataTypes.INTEGER,
      allowNull: true
    }
  }, {
    tableName: 'trsPurchase'
  });
};
