/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsPricelistInstance, trsPricelistAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsPricelistInstance, trsPricelistAttribute>('trsPricelist', {
    plId: {
      type: DataTypes.BIGINT,
      allowNull: false
    },
    plName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    prodId: {
      type: DataTypes.BIGINT,
      allowNull: true
    },
    price: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    }
  }, {
    tableName: 'trsPricelist'
  });
};
