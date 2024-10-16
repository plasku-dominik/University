// minden olyan elemet, amelyet fel szeretnék használni másik állományból és nem beépített a ts-ben azokat importálni kell
import { Sex } from "./Sex"; // a mellete lévő Sex állományból hivatkozzuk be a Sex elemet

export interface IPerson {
    name: string;  // string típusú
    birthDate: Date;
    sex: Sex; 
}