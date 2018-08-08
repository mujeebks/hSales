/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsOrderInstance, trsOrderAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsOrderInstance, trsOrderAttribute>('trsOrder', {
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
      allowNull: true
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
      allowNull: true
    },
    grossAmount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    Discount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    taxableAmount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    taxAmount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    Exempted: {
      type: "MONEY",
      allowNull: true,
      defaultValue: '((0))'
    },
    roundOff: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    roundOffMode: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    cashDiscount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    billAmount: {
      type: DataTypes.FLOAT,
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
    cost: {
      type: DataTypes.FLOAT,
      allowNull: true,
      defaultValue: '((0))'
    },
    SaleValues: {
      type: DataTypes.FLOAT,
      allowNull: true,
      defaultValue: '((0))'
    },
    BillProfit: {
      type: DataTypes.FLOAT,
      allowNull: true,
      defaultValue: '((0))'
    },
    blnProcessed: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    cancelled: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    tinNo: {
      type: DataTypes.STRING,
      allowNull: true
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    },
    AutoNo: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    referenceNo: {
      type: DataTypes.STRING,
      allowNull: true
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
    tableName: 'trsOrder'
  });
};
