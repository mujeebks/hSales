/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {mtrItemImageInstance, mtrItemImageAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<mtrItemImageInstance, mtrItemImageAttribute>('mtrItemImage', {
    itemId: {
      type: DataTypes.BIGINT,
      allowNull: false,
      primaryKey: true
    },
    itemImage: {
      type: "IMAGE",
      allowNull: true
    }
  }, {
    tableName: 'mtrItemImage'
  });
};
