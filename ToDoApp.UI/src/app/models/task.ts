import { Category } from "./category";

export interface Task {
  id: string;
  title: string;
  description?: string;
  dueDate: Date;
  isCompleted?: boolean;
  category: Category;
}
