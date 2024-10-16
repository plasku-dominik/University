"use strict";
// export: azokat az elemeket, amelyeket nyilvánossá akarunk tenni, más állományok számára export kulcsszval látjuk el
// -> későbbiekben az exportált elemet először importálni kell a használatához
Object.defineProperty(exports, "__esModule", { value: true });
exports.Sex = void 0;
// felsorolás: enum, majd név, és felsorolom az értékeket
var Sex;
(function (Sex) {
    Sex[Sex["MALE"] = 0] = "MALE";
    Sex[Sex["FEMALE"] = 1] = "FEMALE";
})(Sex || (exports.Sex = Sex = {}));
