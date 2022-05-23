export interface Food{
    id: string,
    name: string,
    description: string,
    price: number,
    calories: number,
    imagePath: string
}

export interface CreateFood{
    name: string,
    description: string,
    price: number,
    calories: number,
    imagePath: string
}