/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {mtrCurrencyInstance, mtrCurrencyAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<mtrCurrencyInstance, mtrCurrencyAttribute>('mtrCurrency', {
    id: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true
    },
    curName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    majorSymbol: {
      type: DataTypes.STRING,
      allowNull: true
    },
    minorSymbol: {
      type: DataTypes.STRING,
      allowNull: true
    },
    exchangeRate: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    inwordsMode: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    Isdefault: {
      type: DataTypes.BOOLEAN,
      allowNull: true
    }
  }, {
    tableName: 'mtrCurrency'
  });
};
