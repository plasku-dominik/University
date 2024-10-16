import { IPerson } from "./IPersons";
import { Sex } from "./Sex";


class Employee implements IPerson {
    // ts-ben az alapértelmezés szerint public a védelmi szint
    // minden olyan mezőt, amely nem vehet fel undefinied értéket kötelezően inicializálni kell (deklarációkor, vagy konstruktorból)

    name : string;
    birthDate : Date;
    sex : Sex; // interace-t implementáltunk, azaz már van publikus felület
    ssn : string; // taj szám kötelező (nem vehet fel undefine-ot)
    email : string; // email cím, kötelező szöveg

    phoneNumbers : string[]; // telefonszám, string elemek tömbje (lehet 0 eleme, de végtelen sok is)
    constructor (name : string, birthDate : Date, sex : Sex, ssn : string, email : string, phoneNumber? : string) {
        // a példányszíntű elemekre mindig this referenciával hivatkozunk
        this.name = name;
        this.birthDate = birthDate;
        this.sex = sex;
        this.ssn = ssn;
        this.email = email;
        
        this.phoneNumbers = []; // inicializálom a tömböt, 0 elemmel
        // opcionális paraméterek értéke nem undefined, ha beállítják
        if (phoneNumber != undefined) this.phoneNumbers.push(phoneNumber);
        
    }
}