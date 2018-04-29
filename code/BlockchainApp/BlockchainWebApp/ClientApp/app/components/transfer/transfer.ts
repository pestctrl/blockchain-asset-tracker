export interface Transfer {
    $class: string;
    package: string;
    handler: string;
    ingress: boolean;
    longitude: number;
    latitude: number;
    transactionId: string;
    timestamp: Date;
}