export interface Product {
  productId: number;
  productName: string;
  productCode: string;
  description: string;
  unitPrice: number;
  status: string;
  createdAt: string;
}

export interface ProductRequest {
  productName: string;
  productCode: string;
  description: string;
  unitPrice: number;
  status: string;
}
