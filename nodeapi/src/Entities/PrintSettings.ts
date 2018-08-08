/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {PrintSettingsInstance, PrintSettingsAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<PrintSettingsInstance, PrintSettingsAttribute>('PrintSettings', {
    Id: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true
    },
    InvoiceType: {
      type: DataTypes.STRING,
      allowNull: true
    },
    SchemeName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    DosMode: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    paperWidth: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    Leftmargin: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    paperFeed: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    reverseFeed: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    footerLines: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    fontIndex: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    fontSizeIndex: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    Nooflines: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    blnVarlines: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    blnNormal: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    printer: {
      type: DataTypes.STRING,
      allowNull: true
    },
    blnprintValues: {
      type: DataTypes.STRING,
      allowNull: true
    },
    gridColHeadWidth: {
      type: DataTypes.STRING,
      allowNull: true
    },
    printtextvalues: {
      type: DataTypes.STRING,
      allowNull: true
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    }
  }, {
    tableName: 'PrintSettings'
  });
};
