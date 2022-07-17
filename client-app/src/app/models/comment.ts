export interface ChatComment {
    id: number;
    createdAt: Date;
    body: string;
    username: string;
    displayName: string;
    image: string;
}

export interface SendCommentDto {
    activityId: string;
    body: string;
}