export interface GameAPI {
    id:                 number;
    cover:              Cover;
    genres:            Genre[];
    involved_companies: InvolvedCompany[];
    name:               string;
    platforms:          Platform[];
    rating:             number;
    release_dates:      ReleaseDate[];
    summary:            string;
}

export interface Cover {
    id:  number;
    url: string;
}

export interface Genre {
    id:   number;
    name: string;
}

export interface InvolvedCompany {
    id:      number;
    company: Company;
}

export interface Company{
    id: number;
    name: string;
}

export interface Platform{
    id: number;
    name: string;
}

export interface ReleaseDate {
    id:    number;
    human: string;
}
