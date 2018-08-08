/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {InvoiceSettingsInstance, InvoiceSettingsAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<InvoiceSettingsInstance, InvoiceSettingsAttribute>('InvoiceSettings', {
    InvoiceId: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true
    },
    InvoiceType: {
      type: DataTypes.STRING,
      allowNull: true
    },
    FreeQty: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    ShowPreview: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    PrintonSave: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    showConfirm: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    columnWidth: {
      type: DataTypes.STRING,
      allowNull: true
    },
    defPrintScheme: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    editSaleRate: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    blnExtraCol: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    extraColName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    extraColMode: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    },
    AmtDec: {
      type: DataTypes.INTEGER,
      allowNull: true
    }
  }, {
    tableName: 'InvoiceSettings'
  });
};
