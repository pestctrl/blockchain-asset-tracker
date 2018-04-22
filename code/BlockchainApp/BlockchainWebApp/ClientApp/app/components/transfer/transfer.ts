export interface Transfer {
    $class: string;
    package: string;
    origHandler: string;
    newHandler: string;
    longitude: number;
    latitude: number;
    transactionId: string;
    timestamp: Date;
}